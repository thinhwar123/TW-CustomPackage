using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TW.Utility.Extension;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
#endif

namespace TW.UI.CustomStyleSheet
{
    [System.Serializable]
    public class ASelector
    {
        [field: SerializeField] public string TypeSelector { get; set; }
        [field: SerializeField] public string ClassSelector { get; set; }
        [field: SerializeField] public string NameSelector { get; set; }
        [field: SerializeField] public string StateSelector { get; set; }
        public string Tag =>
            $"<color=#BD91F2>{TypeSelector}</color>" +
            $"{(ClassSelector.IsNullOrWhitespace() ? "" : $" <color=#36CB78>.{ClassSelector}</color>")}" +
            $"{(NameSelector.IsNullOrWhitespace() ? "" : $" <color=#B89951>#{NameSelector}</color>")}" +
            $"{(StateSelector.IsNullOrWhitespace() ? "" : $" <color=#BD91F2>:{StateSelector.ToLower()}</color>")}";

        public ASelector()
        {
            
        }
        public ASelector(ASelector other)
        {
            TypeSelector = other.TypeSelector;
            ClassSelector = other.ClassSelector;
            NameSelector = other.NameSelector;
            StateSelector = other.StateSelector;
        }
        public bool IsMatch(ASelector other)
        {
            return TypeSelector == other.TypeSelector &&
                   ClassSelector == other.ClassSelector &&
                   NameSelector == other.NameSelector &&
                   StateSelector == other.StateSelector;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true)]
    public sealed class ASelectorEditorAttribute : System.Attribute
    {
    }
#if UNITY_EDITOR
    public sealed class ASelectorEditorAttributeDrawer : OdinAttributeDrawer<ASelectorEditorAttribute, ASelector>
    {
        private ASelector ConfigValue { get; set; }
        private AVisualElement.Type TypeSelectorFromString(string typeSelector)
        {
            return System.Enum.TryParse(typeSelector, out AVisualElement.Type type)
                ? type
                : AVisualElement.Type.VisualElement;
        }
        private AVisualElement.State StateSelectorFromString(string stateSelector)
        {
            return System.Enum.TryParse(stateSelector, out AVisualElement.State state)
                ? state
                : AVisualElement.State.Default;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ConfigValue = new ASelector(this.ValueEntry.SmartValue);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            using (new GUIColorScope(CColor.lightPurple))
            {
                ConfigValue.TypeSelector = EditorGUI.EnumPopup(rect.AlignLeft(rect.width * 0.24f),
                    TypeSelectorFromString(ConfigValue.TypeSelector)).ToString();
            }

            ConfigValue.ClassSelector = EditorGUI.TextField(rect.AlignLeft(rect.width * 0.49f).SubXMax(rect.width * 0.05f).AlignRight(rect.width * 0.18f), ConfigValue.ClassSelector).ToCamelCase();
            using (new GUIColorScope(CColor.lightGreen))
            {
                SelectPresetButton(rect.AlignLeft(rect.width * 0.49f).AlignRight(rect.width * 0.045f), 
                    AStyleSheetGlobalConfig.Instance.GetAllClassSelector(), 
                    result => { ConfigValue.ClassSelector = result; });
            }

            ConfigValue.NameSelector = EditorGUI.TextField(rect.AlignLeft(rect.width * 0.74f).SubXMax(rect.width * 0.05f).AlignRight(rect.width * 0.18f), ConfigValue.NameSelector).ToCamelCase();
            using (new GUIColorScope(CColor.lightYellow))
            {
                SelectPresetButton(rect.AlignLeft(rect.width * 0.74f).AlignRight(rect.width * 0.045f),
                    AStyleSheetGlobalConfig.Instance.GetAllNameSelector(),
                    result => { ConfigValue.NameSelector = result; });
            }
            
            using (new GUIColorScope(CColor.lightPurple))
            {
                
                ConfigValue.StateSelector = EditorGUI.EnumPopup(rect.AlignRight(rect.width * 0.24f),
                    StateSelectorFromString(ConfigValue.StateSelector)).ToString();
            }

            if (this.ValueEntry.SmartValue.IsMatch(ConfigValue)) return;
            this.ValueEntry.WeakValues.ForceMarkDirty();
            this.ValueEntry.SmartValue = new ASelector(ConfigValue);
        }
        private void SelectPresetButton(Rect rect, string[] menu, UnityAction<string> onSelectedCallback)
        {
            if (!GUI.Button(rect, "-")) return;
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("None"), false, () => onSelectedCallback?.Invoke(""));
            for (int i = 0; i < menu.Length; i++)
            {
                int index = i;
                genericMenu.AddItem(new GUIContent(menu[i]), false, () => onSelectedCallback?.Invoke(menu[index]));
            }
            genericMenu.ShowAsContext();
        }
        

    }
#endif
}