using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;
using System.Collections.Generic;
using TW.Utility.Extension;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
#endif
namespace TW.UI.CustomStyleSheet
{
    [CreateAssetMenu(fileName = "AStyleSheetGlobalConfig", menuName = "GlobalConfigs/AStyleSheetGlobalConfig")]
    [GlobalConfig("Assets/Resources/GlobalConfig/")]
    public class AStyleSheetGlobalConfig : GlobalConfig<AStyleSheetGlobalConfig>
    {
        [field: SerializeField]
        public AStyleSheetConfig[] AStyleSheetConfigs { get; private set; } = new AStyleSheetConfig[]
        {
            new AStyleSheetConfig()
            {
                Selector = new ASelector()
                {
                    TypeSelector = "VisualElement",
                    ClassSelector = "Class",
                    NameSelector = "Name"
                },
                Properties = new List<AProperty>()
                { 
                    new AProperty()
                    {
                        PropertyName = "width",
                    },
                    new AProperty()
                    {
                        PropertyName = "height",
                    },
                }
            }
        };
        public AProperties GetProperties(ASelector selector)
        {
            AProperties properties = new AProperties();
            AStyleSheetConfig typeConfig = AStyleSheetConfigs.FirstOrDefault(config => 
                config.Selector.TypeSelector == selector.TypeSelector && 
                config.Selector.NameSelector.IsNullOrWhitespace() && 
                config.Selector.ClassSelector.IsNullOrWhitespace());
            properties.TryAddProperties(typeConfig?.Properties);
            if (selector.ClassSelector.IsNullOrWhitespace()) return properties;
            AStyleSheetConfig classConfig = AStyleSheetConfigs.FirstOrDefault(config => 
                config.Selector.TypeSelector == selector.TypeSelector && 
                config.Selector.ClassSelector == selector.ClassSelector && 
                config.Selector.NameSelector.IsNullOrWhitespace());
            properties.TryAddProperties(classConfig?.Properties);
            if (selector.NameSelector.IsNullOrWhitespace()) return properties;
            AStyleSheetConfig nameConfig = AStyleSheetConfigs.FirstOrDefault(config => 
                config.Selector.TypeSelector == selector.TypeSelector && 
                config.Selector.ClassSelector == selector.ClassSelector && 
                config.Selector.NameSelector == selector.NameSelector);
            properties.TryAddProperties(nameConfig?.Properties);
            return properties;
        }
        public string[] GetAllClassSelector()
        {
            return AStyleSheetConfigs.Select(config => config.Selector.ClassSelector)
                .Where(s => !s.IsNullOrWhitespace()).Distinct().ToArray();
        }
        public string[] GetAllNameSelector()
        {
            return AStyleSheetConfigs.Select(config => config.Selector.NameSelector)
                .Where(s => !s.IsNullOrWhitespace()).Distinct().ToArray();
        }
        
#if UNITY_EDITOR
        private AVisualElement[] SceneVisualElements { get; set; }
        private AVisualElement[] PrefabVisualElements { get; set; }
        [Button]
        public void UpdateAllStyleSheet()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            SceneVisualElements = FindObjectsByType<AVisualElement>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(x => PrefabUtility.GetCorrespondingObjectFromOriginalSource(x) == null).ToArray();
            SceneVisualElements.ForEach(x => x.UpdateStyleSheet());
            
            string[] findAssets = AssetDatabase.FindAssets($"t:Prefab");
            if (findAssets.Length == 0) return;
            AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(findAssets[0]));
            PrefabVisualElements = findAssets.Select(s => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(s)))
                .SelectMany(s => s.GetComponentsInChildren<AVisualElement>(true))
                .Where(x => PrefabUtility.GetCorrespondingObjectFromOriginalSource(x) == x)
                .ToArray();
            string[] assetPaths = PrefabVisualElements.Select(x => AssetDatabase.GetAssetPath(x.gameObject)).ToArray();
            assetPaths.ForEach((x, i) =>
            {
                using PrefabUtility.EditPrefabContentsScope editingScope = new PrefabUtility.EditPrefabContentsScope(x);
                AVisualElement[] elements = editingScope.prefabContentsRoot.GetComponentsInChildren<AVisualElement>(true)
                    .Where(s => PrefabUtility.GetCorrespondingObjectFromOriginalSource(s) == null)
                    .ToArray();
                elements.ForEach(e =>
                {
                    EditorUtility.SetDirty(e);
                    e.UpdateStyleSheet();
                });
            });
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.SetDirty(this);
        }

        [OnInspectorInit]
        public void OnInspectorInit()
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }
    [System.Serializable]
    public class AStyleSheetConfig
    {
        [field: ShowIf("@IsEditing")]
        [field: SerializeField, ASelectorEditor] public ASelector Selector {get; set;}
        [field: ShowIf("@IsEditing")]
        [field: SerializeField] public AProperties Properties {get; set;}
        private bool IsEditing {get; set;} = false;
#if UNITY_EDITOR
        [OnInspectorGUI, PropertyOrder(-1)]
        private void OnInspectorGUI()
        {            
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label) {richText = true};
            Rect rect = EditorGUILayout.GetControlRect();


            using (new GUIColorScope(CColor.lightGreen))
            {
                EditorGUI.LabelField(rect, "<b>Unity Style Sheet</b>", labelStyle);
                if (GUI.Button(rect.AlignRight(rect.width * 0.2f), IsEditing ? "Save" : "Edit"))
                {
                    IsEditing = !IsEditing;
                }
            }
            if (IsEditing) return;
            if (Properties.Count == 0) return;
            rect = EditorGUILayout.GetControlRect();
            EditorGUI.LabelField(rect, $"{Selector.Tag} {{", labelStyle);
            foreach (AProperty property in Properties.Properties)
            {
                rect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(rect, $"    <color={CColor.blue.ToHex()}>{property.PropertyName}</color>: " +
                                           $"<color={CColor.lightPurple.ToHex()}>{(property.IsAutoProperty() ? "" : property.GetUSSPropertyValue())}</color>" +
                                           $"<color={CColor.lightPink.ToHex()}>{property.StringUnit}</color>", labelStyle);
            }
            rect = EditorGUILayout.GetControlRect();
            EditorGUI.LabelField(rect, "}");
        }
#endif
    }
}