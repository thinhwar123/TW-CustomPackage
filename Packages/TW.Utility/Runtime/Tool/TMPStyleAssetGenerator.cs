using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

namespace TW.Utility.Tool
{
    [CreateAssetMenu(fileName = "TMPStyleAssetGenerator", menuName = "TW/Utility/TMPStyleAssetGenerator")]

    public class TMPStyleAssetGenerator : ScriptableObject
    {
#if UNITY_EDITOR
        [field: SerializeField] public TMP_StyleSheet TMPStyleSheet { get; private set; }
        [field: SerializeField] public TMPStyleConfig[] TMPStyleConfigs { get; private set; }

        [Button]
        public void GenerateTMPStyleAsset()
        {
            EditorUtility.SetDirty(TMPStyleSheet);
            SerializedObject serializedObject = new SerializedObject(TMPStyleSheet);
            SerializedProperty styleList = serializedObject.FindProperty("m_StyleList");
            ClearSpriteStyle(styleList);
            int currentArraySize = styleList.arraySize;
            styleList.arraySize = currentArraySize + TMPStyleConfigs.Length;
            
            for (int i = 0; i < TMPStyleConfigs.Length; i++)
            {
                int index = i + currentArraySize;
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_Name").stringValue = TMPStyleConfigs[i].Style;
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_HashCode").intValue = TMP_TextParsingUtilities.GetHashCode(TMPStyleConfigs[i].Style);
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_OpeningDefinition").stringValue = $"<sprite index={TMPStyleConfigs[i].Index}>";
                styleList.GetArrayElementAtIndex(index).FindPropertyRelative("m_ClosingDefinition").stringValue = "";
            }
            serializedObject.ApplyModifiedProperties();
            TMPStyleSheet.RefreshStyles();
            string[] path = SceneManager.GetActiveScene().path.Split(char.Parse("/"));
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), string.Join("/", path));
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
#endif
    }
    
    [System.Serializable]
    public class TMPStyleConfig
    {
        [field: SerializeField] public string Style {get; private set;}
        [field: SerializeField] public int Index {get; private set;}
    }
}
