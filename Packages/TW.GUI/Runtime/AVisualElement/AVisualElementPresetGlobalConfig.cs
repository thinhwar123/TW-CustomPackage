using System.Linq;
using UnityEngine;
using Sirenix.Utilities;

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
#endif

namespace TW.GUI
{
    [CreateAssetMenu(fileName = "AVisualElementPresetGlobalConfig",
        menuName = "GlobalConfigs/AVisualElementPresetGlobalConfig")]
    [GlobalConfig("Assets/Resources/GlobalConfig/")]
    public class AVisualElementPresetGlobalConfig : GlobalConfig<AVisualElementPresetGlobalConfig>
    {
#if UNITY_EDITOR
        [ShowInInspector, PropertyOrder(-1), FoldoutGroup("Path")]
        public string TextPresetPath => "Assets/Editor/UIPresets/TextPresets";

        [ShowInInspector, PropertyOrder(-1), FoldoutGroup("Path")]
        public string ButtonPresetPath => "Assets/Editor/UIPresets/ButtonPresets";

        [ShowInInspector, PropertyOrder(-1), FoldoutGroup("Path")]
        public string RectPresetPath => "Assets/Editor/UIPresets/RectPresets";

        [ShowInInspector, PropertyOrder(-1), FoldoutGroup("Path")]
        public string ImagePresetPath => "Assets/Editor/UIPresets/ImagePresets";

        [ShowInInspector, PropertyOrder(-1), FoldoutGroup("Path")]
        public string MainUIPath => "Assets/Resources/UI";

        [field: SerializeField] public APreset[] TextPresets { get; private set; }
        [field: SerializeField] public APreset[] ButtonPresets { get; private set; }
        [field: SerializeField] public APreset[] RectPresets { get; private set; }
        [field: SerializeField] public APreset[] ImagePresets { get; private set; }

        [Button(SdfIconType.ArrowRepeat, ButtonHeight = 40), HorizontalGroup("Action")]
        public void UpdateAllPreset()
        {
            CreateDirectoryRecursively(TextPresetPath);
            CreateDirectoryRecursively(ButtonPresetPath);
            CreateDirectoryRecursively(RectPresetPath);
            CreateDirectoryRecursively(ImagePresetPath);
            TextPresets = AssetDatabase.FindAssets("t:Prefab", new string[] { TextPresetPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<APreset>)
                .ToArray();
            ButtonPresets = AssetDatabase.FindAssets("t:Prefab", new string[] { ButtonPresetPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<APreset>)
                .ToArray();
            RectPresets = AssetDatabase.FindAssets("t:Prefab", new string[] { RectPresetPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<APreset>)
                .ToArray();
            ImagePresets = AssetDatabase.FindAssets("t:Prefab", new string[] { ImagePresetPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<APreset>)
                .ToArray();
        }

        [Button(SdfIconType.CloudArrowDown, ButtonHeight = 40), HorizontalGroup("Action")]
        public void ApplyAllPreset()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AVisualElement[] sceneVisualElements =
                FindObjectsByType<AVisualElement>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .Where(x => PrefabUtility.GetCorrespondingObjectFromOriginalSource(x) == null).ToArray();
            sceneVisualElements.ForEach(x => x.ApplyPreset());

            CreateDirectoryRecursively(MainUIPath);
            List<AVisualElement> applyPrefabVisualElements = new List<AVisualElement>();
            string[] assetPaths = AssetDatabase.FindAssets("t:Prefab", new string[] { MainUIPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();
            assetPaths.ForEach(x =>
            {
                using PrefabUtility.EditPrefabContentsScope editingScope = new PrefabUtility.EditPrefabContentsScope(x);

                AVisualElement[] elements = editingScope.prefabContentsRoot
                    .GetComponentsInChildren<AVisualElement>(true)
                    .Where(s => PrefabUtility.GetCorrespondingObjectFromOriginalSource(s) == null)
                    .ToArray();
                elements.ForEach(e =>
                {
                    if (applyPrefabVisualElements.Contains(e)) return;
                    EditorUtility.SetDirty(e);
                    e.ApplyPreset();
                    applyPrefabVisualElements.Add(e);
                });
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void CreateDirectoryRecursively(string path)
        {
            string parentDirectory = Path.GetDirectoryName(path);

            // If the parent directory does not exist...
            if (!AssetDatabase.IsValidFolder(parentDirectory))
            {
                // Recursively create the parent directory
                CreateDirectoryRecursively(parentDirectory);
            }

            // Now that we've made sure the parent directory exists, 
            // We can safely create the directory without worrying about missing parent directories
            if (!AssetDatabase.IsValidFolder(path))
            {
                string newFolderName = Path.GetFileName(path);
                string newFolderPath = Path.GetDirectoryName(path);
                AssetDatabase.CreateFolder(newFolderPath, newFolderName);
            }
        }

        public APreset[] GetPresets(AVisualElement.EType visualElementType)
        {
            return visualElementType switch
            {
                AVisualElement.EType.Text => TextPresets,
                AVisualElement.EType.Image => ImagePresets,
                AVisualElement.EType.Button => ButtonPresets,
                AVisualElement.EType.Rect => RectPresets,
                _ => null
            };
        }

        public List<APresetProperty.Type> GetPresetPropertiesType(AVisualElement.EType visualElementType)
        {
            return Enum.GetValues(typeof(APresetProperty.Type)).Cast<APresetProperty.Type>()
                .Where(preset => preset.ToString().Contains(visualElementType.ToString()))
                .ToList();
        }
#endif
    }
}