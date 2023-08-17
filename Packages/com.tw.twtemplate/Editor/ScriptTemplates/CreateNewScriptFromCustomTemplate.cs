using UnityEngine;
using UnityEditor;

namespace TW.Template
{
    public class CreateNewScriptFromCustomTemplate
    {
        #region Scriptable Object Scripts
        [MenuItem(itemName: "Assets/CustomTemplate/Create New Scriptable Object", isValidateFunction: false, priority: 1)]
        public static void CreateScriptScriptableObjectFromTemplate()
        {
            CreateScriptFromTemplate("NewScriptableObjectTemplate.cs", "NewScriptableObject.cs");
        }
        [MenuItem(itemName: "Assets/CustomTemplate/Create New Global Config", isValidateFunction: false, priority: 1)]
        public static void CreateScriptGlobalDataFromTemplate()
        {
            CreateScriptFromTemplate("NewGlobalConfigTemplate.cs", "NewGlobalConfig.cs");
        }
        #endregion

        #region ECS Scripts
        [MenuItem(itemName: "Assets/CustomTemplate/ECS/Create New ComponentData", isValidateFunction: false, priority: 1)]
        public static void CreateScriptComponentDataFromTemplate()
        {
            CreateScriptFromTemplate("NewComponentDataTemplate.cs", "NewComponentData.cs");
        }
        [MenuItem(itemName: "Assets/CustomTemplate/ECS/Create New Authoring", isValidateFunction: false, priority: 1)]
        public static void CreateScriptAuthoringFromTemplate()
        {
            CreateScriptFromTemplate("NewAuthoringTemplate.cs", "NewAuthoring.cs");
        }
        [MenuItem(itemName: "Assets/CustomTemplate/ECS/Create New System", isValidateFunction: false, priority: 1)]
        public static void CreateScriptSystemFromTemplate()
        {
            CreateScriptFromTemplate("NewSystemTemplate.cs", "NewSystem.cs");
        }

        [MenuItem(itemName: "Assets/CustomTemplate/ECS/Create New Aspect", isValidateFunction: false, priority: 1)]
        public static void CreateScriptAspectFromTemplate()
        {
            CreateScriptFromTemplate("NewAspectTemplate.cs", "NewAspect.cs");
        }

        /// TO BE CONTINUE ... DEVELOP
        //[MenuItem(itemName: "Assets/StateMachine ECS/Create New State System", isValidateFunction: false, priority: 1)]
        //public static void CreateScriptStateSystemFromTemplate()
        //{
        //    CreateScriptFromTemplate("NewStateSystemTemplate.cs", "NewStateSystem.cs");
        //} 
        #endregion

        public static void CreateScriptFromTemplate(string templateName, string defaultFileName)
        {
            string[] findAssets = AssetDatabase.FindAssets($"t:TextAsset {templateName}");

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GUIDToAssetPath(findAssets[0]), defaultFileName);
        }
    }

}