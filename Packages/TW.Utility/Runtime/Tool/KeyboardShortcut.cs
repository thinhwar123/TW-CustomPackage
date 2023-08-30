#if UNITY_EDITOR
using UnityEditor;
#endif 

namespace TW.Utility.Tool
{
#if UNITY_EDITOR
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
}