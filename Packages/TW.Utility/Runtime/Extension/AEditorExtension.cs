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
        public const string FormatGetColorInPalette = "@TW.Utility.Extension.AEditorExtension.GetColorInPalette(\"{0}\", (int)$value)";
        public const string FormatGetColorById = "@TW.Utility.Extension.AEditorExtension.GetColorById((int)$value, {0})";
        public const string FormatGetColorInGlobalConfig = "@TW.Utility.Extension.AEditorExtension.GetColorInGlobalConfig(\"{0}\", (int)$value)";

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
        /// <param name="mono">The MonoBehaviour object to check if it’s a part of the current prefab stage.</param>
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

