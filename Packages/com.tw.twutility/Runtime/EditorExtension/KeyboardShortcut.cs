
namespace TW.Utility
{
#if UNITY_EDITOR
using UnityEditor;

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