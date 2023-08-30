using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace TW.Utility.Extension
{
    public static class AEditorExtension
    {
        /// <summary>
        /// Returns a boolean indicating whether the current stage of the editor is a prefab stage.
        /// </summary>
        /// <returns></returns>
        public static bool IsInPrefabStage()
        {
#if UNITY_EDITOR
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
            return stage != null;
#else
            return true;
#endif
        }
        /// <summary>
        /// Returns a boolean indicating whether the specified MonoBehaviour object is a part of the current prefab stage and not a persistent object in the scene.
        /// </summary>
        /// <param name="mono">The MonoBehaviour object to check if it�s a part of the current prefab stage.</param>
        /// <returns></returns>
        public static bool IsInPrefabStage(this MonoBehaviour mono)
        {
#if UNITY_EDITOR
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            return stage != null && stage.prefabContentsRoot.name == mono.name && !EditorUtility.IsPersistent(mono.gameObject);
#else
            return true;
#endif
        }
        /// <summary>
        /// Searches for a prefab asset with the specified name and returns the first result if found.
        /// </summary>
        /// <param name="name">The name of the prefab asset to search for.</param>
        /// <returns></returns>
        public static GameObject FindPrefab(string name)
        {
#if UNITY_EDITOR
            string[] findAssets = AssetDatabase.FindAssets($"t:Prefab {name}");
            return findAssets.Length > 0 ? AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(findAssets[0])) : null;
#else
            return null;
#endif
        }
        /// <summary>
        /// Searches for a model asset with the specified name and returns the first result if found.
        /// </summary>
        /// <param name="name">The name of the model asset to search for.</param>
        /// <returns></returns>
        public static GameObject FindModel(string name)
        {
#if UNITY_EDITOR
            string[] findAssets = AssetDatabase.FindAssets($"t:Model {name}");
            return findAssets.Length > 0 ? AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(findAssets[0])) : null;
#else
            return null;
#endif
        }
        /// <summary>
        /// Retrieves a color from the specified color palette with the specified index.
        /// Returns white if the color palette or index cannot be found.
        /// This method requires the Sirenix.OdinInspector.Editor namespace.
        /// </summary>
        /// <param name="colorPaletteName">The name of the color palette to search for the color in.</param>
        /// <param name="index">The index of the color in the color palette to retrieve.</param>
        /// <returns></returns>
        public static Color GetColorInPalette(string colorPaletteName, int index)
        {
#if UNITY_EDITOR
            Sirenix.OdinInspector.Editor.ColorPalette colorPalette = Sirenix.OdinInspector.Editor.ColorPaletteManager.Instance.ColorPalettes.FirstOrDefault(x => x.Name == colorPaletteName);
            return colorPalette != null ? colorPalette.Colors[index] : Color.white;
#else
            return Color.white;
#endif
        }
        /// <summary>
        /// Returns a color generated based on the provided ID.
        /// Colors with IDs closer to each other will have a greater difference. White text is easily visible on the background color.
        /// </summary>
        /// <param name="id">The ID used to generate the color.</param>
        /// <param name="range">The number of the color.</param>
        /// <returns>A color generated based on the provided ID.</returns>
        public static Color GetColorById(int id, int range)
        {
            // Define the range of hues for the colors (0-360 degrees)
            float hueRange = 360f;

            // Calculate the hue for this ID (wrapping around after 100 IDs)
            float hue = Mathf.Repeat((float)id / range * hueRange, hueRange);

            // Define the saturation and value for the colors
            float saturation = 0.7f;
            float value = 0.9f;

            // Adjust the hue based on neighboring IDs to increase the difference
            float hueAdjustment = Mathf.Clamp01(1f / 100f * hueRange);
            hue += hueAdjustment * ((id / 10) % 2 == 0 ? 1 : -1);

            // Create the color from the hue, saturation, and value
            Color color = Color.HSVToRGB(hue / hueRange, saturation, value);

            return color;
        }

        public static Color GetColorInGlobalConfig(string colorPaletteName, int index)
        {

            return EditorColorGlobalConfig.Instance.GetColor(colorPaletteName, index);
        }
        /// <summary>
        /// Finds all prefabs within the specified folders.
        /// </summary>
        /// <param name="searchInFolders">The folders to search for prefabs in.</param>
        /// <returns>An array of GameObjects representing the found prefabs. Returns null if not in the Unity Editor.</returns>
        public static GameObject[] FindPrefabInFolders(params string[] searchInFolders)
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:Prefab", searchInFolders);
            return guids.Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(x))).ToArray();
#else
            return null;
#endif
        }
        /// <summary>
        /// Finds the asset paths of all prefabs within the specified folders.
        /// </summary>
        /// <param name="searchInFolders">The folders to search for prefabs in.</param>
        /// <returns>An array of strings representing the asset paths of the found prefabs. Returns null if not in the Unity Editor.</returns>
        public static string[] FindPrefabAssetPathInFolders(params string[] searchInFolders)
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:Prefab", searchInFolders);
            return guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
#else
            return null;
#endif
        }
    }
}

