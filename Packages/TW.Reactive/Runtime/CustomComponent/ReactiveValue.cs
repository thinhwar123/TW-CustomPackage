using System;
using System.Linq;
using MemoryPack;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace TW.Reactive.CustomComponent
{
    [MemoryPackable]
    [Serializable]
    public partial class ReactiveValue<T>
    {
#if UNITY_EDITOR
        public class ReactiveStructValueDrawer : OdinValueDrawer<ReactiveValue<T>> 
        {
            protected override void DrawPropertyLayout(GUIContent label)
            {
                ValueEntry.Property.Children.First().Draw(label);
            }
        }
#endif
        [OnValueChanged(nameof(OnValueChange))]
        [Delayed]
        [SerializeField] private T m_Value;
        public T Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                ReactiveProperty.Value = value;
            }
        }
        [MemoryPackIgnore]
        public ReactiveProperty<T> ReactiveProperty {get; private set;}

        public ReactiveValue()
        {
            ReactiveProperty = new ReactiveProperty<T>(m_Value);
            
        }
        [MemoryPackConstructor]
        public ReactiveValue(T value)
        {
            m_Value = value;
            ReactiveProperty = new ReactiveProperty<T>(value);
            Value = value;
        }
        
        public static implicit operator T(ReactiveValue<T> reactiveValue)
        {
            return reactiveValue.Value;
        }

        private void OnValueChange()
        {
#if UNITY_EDITOR
            ReactiveProperty.Value = m_Value;
#endif
        }
    }
}