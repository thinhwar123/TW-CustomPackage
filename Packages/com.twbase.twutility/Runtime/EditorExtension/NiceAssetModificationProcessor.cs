namespace TW.Utility
{
    using UnityEngine;
    using System;
    using System.IO;
    using UnityEditor;
    using static UnityEditor.Progress;


    public class NiceAssetModificationProcessor : AssetModificationProcessor
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            //// Get the name of the scene to save.
            //string scenePath = string.Empty;
            //string sceneName = string.Empty;

            //foreach (string path in paths)
            //{
            //    if (!path.Contains(".unity")) continue;
            //    scenePath = Path.GetDirectoryName(path);
            //    sceneName = Path.GetFileNameWithoutExtension(path);
            //}

            //if (sceneName.Length == 0)
            //{
            //    return paths;
            //}

            //TODO: Add My Asset Modification Processor
            Debug.Log("Add My Asset Modification Processor");

            return paths;
        }
    } 
}