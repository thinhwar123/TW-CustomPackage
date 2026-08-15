using System;
using System.Reflection;
using Newtonsoft.Json;
using R3;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using UnityEngine;

namespace TW.Reactive.CustomComponent
{
    [System.Serializable]
    public class Reactive<T> : SerializableReactiveProperty<T>
    {
        public Reactive() : base()
        {
        }

        public Reactive(T initialValue) : base(initialValue)
        {
#if UNITY_EDITOR
            EditorValue = initialValue;
#endif
        }

        public static implicit operator T(Reactive<T> reactive) => reactive.Value;


#if UNITY_EDITOR
        [field: OnValueChanged("OnEditorValueChanged")] [field: SerializeField]
        private T EditorValue = default;

        private IDisposable subscription;

        private void OnEditorValueChanged()
        {
            Value = EditorValue;
        }

        public class ReactiveValueDrawer : OdinValueDrawer<Reactive<T>>
        {
            protected override void Initialize()
            {
                base.Initialize();
                ValueEntry.SmartValue.subscription?.Dispose();
                ValueEntry.SmartValue.subscription = ValueEntry.SmartValue.Subscribe(OnValueChanged);
            }

            protected override void DrawPropertyLayout(GUIContent label)
            {
                ValueEntry.Property.Children[1].Draw(label);
            }

            private void OnValueChanged(T value)
            {
                ValueEntry.SmartValue.EditorValue = value;
            }
        }
#endif
    }

    public class ReactiveConverterFactory : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType &&
                   objectType.GetGenericTypeDefinition() == typeof(Reactive<>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            Type valueType = objectType.GetGenericArguments()[0];
            object value = serializer.Deserialize(reader, valueType);
            return Activator.CreateInstance(objectType, value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PropertyInfo prop = value!.GetType().GetProperty("Value");
            serializer.Serialize(writer, prop!.GetValue(value));
        }
    }
}