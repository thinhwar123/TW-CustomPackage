#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace TW.Utility.Editor.Template
{
    public class CreateNewScriptFromCustomTemplate
    {
#if UNITY_EDITOR
        #region Scriptable Object Scripts
        [MenuItem(itemName: "Assets/CustomTemplate/Scriptable Object/Create New Scriptable Object", isValidateFunction: false, priority: 1)]
        public static void CreateScriptScriptableObjectFromTemplate()
        {
            CreateScriptFromTemplate("NewScriptableObjectTemplate.cs", "NewScriptableObject.cs");
        }
        [MenuItem(itemName: "Assets/CustomTemplate/Scriptable Object/Create New Global Config", isValidateFunction: false, priority: 1)]
        public static void CreateScriptGlobalDataFromTemplate()
        {
            CreateScriptFromTemplate("NewGlobalConfigTemplate.cs", "NewGlobalConfig.cs");
        }
        #endregion

        #region Useful Built-in Scripts

        [MenuItem(itemName: "Assets/CustomTemplate/Useful Built-in/Create New Asset Modification Processor", isValidateFunction: false, priority: 1)]
        public static void CreateScriptAssetModificationProcessorFromTemplate()
        {
            CreateScriptFromTemplate("NewMyAssetModificationProcessor.cs", "MyAssetModificationProcessor.cs");
        }

        #endregion
        
        #region MVP Scripts
        
        [MenuItem(itemName: "Assets/CustomTemplate/MVP/Create New Screen", isValidateFunction: false, priority: 1)]
        public static void CreateScriptScreenFromTemplate()
        {
            CreateScriptFromTemplate("NewScreenTemplate.cs", "Screen.cs");
        }
        [MenuItem(itemName: "Assets/CustomTemplate/MVP/Create New Modal", isValidateFunction: false, priority: 1)]
        public static void CreateScriptModalFromTemplate()
        {
            CreateScriptFromTemplate("NewModalTemplate.cs", "Modal.cs");
        }
        [MenuItem(itemName: "Assets/CustomTemplate/MVP/Create New Activity", isValidateFunction: false, priority: 1)]
        public static void CreateScriptActivityFromTemplate()
        {
            CreateScriptFromTemplate("NewActivityTemplate.cs", "Activity.cs");
        }
        
        #endregion

        public static void CreateScriptFromTemplate(string templateName, string defaultFileName)
        {
            string[] findAssets = AssetDatabase.FindAssets($"t:TextAsset {templateName}");

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GUIDToAssetPath(findAssets[0]), defaultFileName);
        } 
#endif
    }

}