using UnityEditor;
using UnityEngine;

namespace TW.GUI
{
    public static class AVisualElementCreator
    {
        [MenuItem("GameObject/VisualElement Creator/A VisualElement", false, 0)]
        public static void CreateAVisualElement()
        {
            CreateAUI("AVisualElement");
        }
        [MenuItem("GameObject/VisualElement Creator/A Text VisualElement", false, 1)]
        public static void CreateATextVisualElement()
        {
            CreateAUI("ATextVisualElement");
        }


        private static void CreateAUI(string nameUI)
        {
            GameObject currentSelectGameObject = Selection.activeObject as GameObject;
            GameObject prefab = FindPrefab(nameUI);
            GameObject item = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            item?.transform.SetParent(currentSelectGameObject?.transform, false);
            item?.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            item?.transform.SetAsLastSibling();
            if (item != null) item.name = nameUI;
            PrefabUtility.UnpackPrefabInstance(item, PrefabUnpackMode.Completely, InteractionMode.UserAction);
            Selection.activeObject = item;
        }

        private static GameObject FindPrefab(string name)
        {
            string[] findAssets = AssetDatabase.FindAssets($"t:Prefab _{name}_");
            return findAssets.Length > 0 ? AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(findAssets[0])) : null;
        }
    }

}