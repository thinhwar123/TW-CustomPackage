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
        /// <summary>
        /// Finds and returns an instance of a specified Unity Object type in the project's assets or scene hierarchy. 
        /// </summary>
        /// <typeparam name="T">The type of Unity Object to find (e.g., GameObject, ScriptableObject).</typeparam>
        /// <param name="name">Optional. The name or partial name of the object to search for.</param>
        /// <returns>
        /// Returns an instance of the specified type if found; otherwise, returns null.
        /// </returns>
        public static T FindObjectOfType<T>(string name = "") where T : Object
        {
#if UNITY_EDITOR
            string objectName = name == "" ? typeof(T).Name : name;
            string[] findAssets = AssetDatabase.FindAssets($"t:{typeof(T).Name} {objectName}");
            return findAssets.Length > 0 ? AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(findAssets[0])) : null;
#else
            return null;
#endif
        }
        
    }
}

