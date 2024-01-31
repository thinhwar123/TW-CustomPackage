using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace TW.Utility.Editor.Sample
{
    public class CreateUISampleGameObject
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/UI Creator/A UI Text", false, 0)]
        public static void CreateAUIText()
        {
            CreateAUI("AUIText");
        }
        [MenuItem("GameObject/UI Creator/A UI Button", false, 0)]
        public static void CreateAUIButton()
        {
            CreateAUI("AUIButton");
        }
        [MenuItem("GameObject/UI Creator/A UI Select Button", false)]
        public static void CreateAUISelectButton()
        {
            CreateAUI("AUISelectButton");
        }
        [MenuItem("GameObject/UI Creator/A UI ProcessBar", false)]
        public static void CreateAUIProcessBar()
        {
            CreateAUI("AUIProcessBar");
        }
        [MenuItem("GameObject/UI Creator/A UI Repeatable Button", false)]
        public static void CreateAUIRepeatableButton()
        {
            CreateAUI("AUIRepeatableButton");
        }
        [MenuItem("GameObject/UI Creator/A UI Switch Toggle Button", false)]
        public static void CreateAUISwitchToggleButton()
        {
            CreateAUI("AUISwitchToggleButton");
        }
        [MenuItem("GameObject/UI Creator/A UI Switch Tab", false)]
        public static void CreateAUISwitchTab()
        {
            CreateAUI("AUISwitchTab");
        }


        private static void CreateAUI(string nameUI)
        {
            GameObject currentSelectGameObject = Selection.activeObject as GameObject;
            GameObject prefab = FindPrefab(nameUI);
            GameObject item = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            item?.transform.SetParent(currentSelectGameObject?.transform, false);
            item?.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            item?.transform.SetAsLastSibling();
            PrefabUtility.UnpackPrefabInstance(item, PrefabUnpackMode.Completely, InteractionMode.UserAction);
            Selection.activeObject = item;
        }

        private static GameObject FindPrefab(string name)
        {
            string[] findAssets = AssetDatabase.FindAssets($"t:Prefab {name}");
            return findAssets.Length > 0 ? AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(findAssets[0])) : null;
        }
#endif
    }

}