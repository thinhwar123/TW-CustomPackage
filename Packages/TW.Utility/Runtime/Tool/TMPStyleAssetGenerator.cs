using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TW.Utility.Tool
{
    [CreateAssetMenu(fileName = "TMPStyleAssetGenerator", menuName = "TW/Utility/TMPStyleAssetGenerator")]

    public class TMPStyleAssetGenerator : ScriptableObject
    {
        [field: SerializeField] public TMP_StyleSheet TMPStyleSheet { get; private set; }
        [field: SerializeField] public TMPStyleConfig[] TMPStyleConfigs { get; private set; }

        [Button]
        public void GenerateTMPStyleAsset()
        {
            SerializedObject serializedObject = new SerializedObject(TMPStyleSheet);
            SerializedProperty styleList = serializedObject.FindProperty("m_StyleList");
            ClearSpriteStyle(styleList);

            int curentArraySize = styleList.arraySize;
            styleList.arraySize = curentArraySize + TMPStyleConfigs.Length;
            
            for (int i = 0; i < TMPStyleConfigs.Length; i++)
            {
                int index = i + curentArraySize;
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_Name").stringValue = TMPStyleConfigs[i].Style;
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_HashCode").intValue = TMP_TextParsingUtilities.GetHashCode(TMPStyleConfigs[i].Style);
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_OpeningDefinition").stringValue = $"<sprite index={TMPStyleConfigs[i].Index}>";
            }
            styleList.serializedObject.ApplyModifiedProperties();
        }

        private void ClearSpriteStyle(SerializedProperty styleList)
        {
            int i = 0;
            int arraySize = styleList.arraySize;
            while (i < arraySize)
            {
                if (styleList.GetArrayElementAtIndex(i).FindPropertyRelative("m_OpeningDefinition").stringValue.Contains("<sprite index="))
                {
                    styleList.DeleteArrayElementAtIndex(i);
                    arraySize--;
                }
                i++;
            }
        }
    }
    
    [System.Serializable]
    public class TMPStyleConfig
    {
        [field: SerializeField] public string Style {get; private set;}
        [field: SerializeField] public int Index {get; private set;}
    }
}
