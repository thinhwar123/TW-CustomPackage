using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.Utility.CustomType
{
    public class NavMeshMaskAttribute : PropertyAttribute { }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(NavMeshMaskAttribute))]
    public class NavMeshMaskDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {

            EditorGUI.PrefixLabel(position.AlignLeft(EditorGUIUtility.labelWidth), label);

            EditorGUI.BeginChangeCheck();

            string[] areaNames = UnityEngine.AI.NavMesh.GetAreaNames().Where(value => value != "").ToArray();
            string[] completeAreaNames = new string[areaNames.Length];

            foreach (string name in areaNames)
            {
                completeAreaNames[UnityEngine.AI.NavMesh.GetAreaFromName(name)] = name;
            }

            int mask = serializedProperty.intValue;

            mask = EditorGUI.MaskField(position.AlignRight(position.width - EditorGUIUtility.labelWidth), mask, completeAreaNames);
            if (EditorGUI.EndChangeCheck())
            {
                serializedProperty.intValue = mask;
            }
        }
    }
    #endif
}