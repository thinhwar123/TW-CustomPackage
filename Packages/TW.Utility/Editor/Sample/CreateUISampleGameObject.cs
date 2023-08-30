using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace TW.Utility.Editor.Sample
{
    public class CreateUISampleGameObject
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/UIHelper/Process Bar", false, 0)]
        public static void TestCreate()
        {
            GameObject currentSelectGameObject = Selection.activeObject as GameObject;
            GameObject prefab = FindPrefab("Process Bar");
            GameObject item = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            item?.transform.SetParent(currentSelectGameObject?.transform, false);
            item?.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            item?.transform.SetAsLastSibling();
            PrefabUtility.UnpackPrefabInstance(item, PrefabUnpackMode.Completely, InteractionMode.UserAction);
            Selection.activeObject = item;
        }

        public static GameObject FindPrefab(string name)
        {
            string[] findAssets = AssetDatabase.FindAssets($"t:Prefab {name}");
            return findAssets.Length > 0 ? AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(findAssets[0])) : null;
        }
#endif
    }

}