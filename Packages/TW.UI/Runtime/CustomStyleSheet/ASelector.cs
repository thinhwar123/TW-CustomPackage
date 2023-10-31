using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using TW.Utility.Extension;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
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
        

        protected override void DrawPropertyLayout(GUIContent label)
        {
            ASelector value = new ASelector(this.ValueEntry.SmartValue);
            Rect rect = EditorGUILayout.GetControlRect();
            using (new GUIColorScope(CColor.lightPurple))
            {
                value.TypeSelector = EditorGUI.EnumPopup(rect.AlignLeft(rect.width * 0.24f),
                    TypeSelectorFromString(value.TypeSelector)).ToString();
            }

            value.ClassSelector = EditorGUI.TextField(rect.AlignLeft(rect.width * 0.49f).SubXMax(rect.width * 0.05f).AlignRight(rect.width * 0.18f), value.ClassSelector).ToCamelCase();
            using (new GUIColorScope(CColor.lightGreen))
            {
                SelectPresetButton(rect.AlignLeft(rect.width * 0.49f).AlignRight(rect.width * 0.045f), 
                    AStyleSheetGlobalConfig.Instance.GetAllClassSelector(), 
                    result => { value.ClassSelector = result; });
            }

            value.NameSelector = EditorGUI.TextField(rect.AlignLeft(rect.width * 0.74f).SubXMax(rect.width * 0.05f).AlignRight(rect.width * 0.18f), value.NameSelector).ToCamelCase();
            using (new GUIColorScope(CColor.lightYellow))
            {
                SelectPresetButton(rect.AlignLeft(rect.width * 0.74f).AlignRight(rect.width * 0.045f),
                    AStyleSheetGlobalConfig.Instance.GetAllNameSelector(),
                    result => { value.NameSelector = result; });
            }
            
            using (new GUIColorScope(CColor.lightPurple))
            {
                value.StateSelector = EditorGUI.EnumPopup(rect.AlignRight(rect.width * 0.24f),
                    StateSelectorFromString(value.StateSelector)).ToString();
            }

            if (this.ValueEntry.SmartValue.IsMatch(value)) return;
            this.ValueEntry.WeakValues.ForceMarkDirty();
            this.ValueEntry.SmartValue = value;
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