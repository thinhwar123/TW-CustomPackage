#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.EditorCode;

public class KeyboardShortcut
{
    [MenuItem("Tools/Editor Windows/PlayerPref Editor Window %#,")]
    private static void OpenPlayerPrefEditorWindow()
    {
        string menuItem = "Tools/Code Stage/🕵 Anti-Cheat Toolkit/Prefs Editor as Tab...";
        EditorApplication.ExecuteMenuItem(menuItem);
    }
}

#endif