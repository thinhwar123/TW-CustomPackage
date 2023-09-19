using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
#endif

// Instructions:
// This package includes support packages.
// After installation, removing this package will help reduce the build size of the file
// and ensure there are no redundant files during the build.
// +++ Open Window -> Package Manager -> TW Help Package -> Remove +++

namespace TW.HelpPackage
{
    public class Notification
    {
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            // This code runs every time scripts are reloaded in the Unity Editor.

            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // Log a warning message when entering play mode.
                Debug.Log("--- Warning: You may need to remove this TW Help Package when build to reduce size. Click here for instructions. ---");
                Debug.Log("--- Warning: You may need to remove this TW Help Package when build to reduce size. Click here for instructions. ---");
                Debug.Log("--- Warning: You may need to remove this TW Help Package when build to reduce size. Click here for instructions. ---");
            }
        }
    }

}