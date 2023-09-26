using System.Collections.Generic;
using Sirenix.Utilities;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TW.UI.CustomComponent;
using UnityEditor;
#endif


#if UNITY_EDITOR
public class UIColorPalettesEditorWindow : OdinMenuEditorWindow
{
    [MenuItem("Tools/Editor Windows/Game Config Editor Window %#/")]
    private static void Open()
    {
        UIColorPalettesEditorWindow window = GetWindow<UIColorPalettesEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;
        tree.Add("Color Palettes", UIColorPalettesGlobalConfig.Instance);
        return tree;
    }
    private void AddAllItemToTree<T>(string menuPath, OdinMenuTree tree, IReadOnlyList<T> objects, System.Func<T, string> getNameFunc)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            tree.AddMenuItemAtPath(menuPath, new OdinMenuItem(tree, getNameFunc(objects[i]), objects[i]));
        }
    }
    
    public void RepaintEditorWindow()
    {
        GUIHelper.CurrentWindow.Repaint();

    }
}
#endif
