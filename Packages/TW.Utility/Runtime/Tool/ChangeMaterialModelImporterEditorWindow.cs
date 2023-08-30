using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.Utility.Tool
{

    #if UNITY_EDITOR
    public class ChangeMaterialModelImporterEditorWindow : EditorWindow
    {

        private readonly Dictionary<string, Material> m_MaterialDictionary = new();
        private List<GameObject> m_SelectedObjects;

        [MenuItem("Tools/Editor Windows/ChangeMaterialModelImporter")]
        public static void OpenEditorWindow()
        {
            ChangeMaterialModelImporterEditorWindow win = ScriptableObject.CreateInstance<ChangeMaterialModelImporterEditorWindow>();
            win.position = new Rect(10, 10, 400, 400);
            win.Show(true);
        }

        public ChangeMaterialModelImporterEditorWindow()
        {

        }
        public void OnSelectionChange()
        {
            this.Repaint();
        }
        public void OnGUI()
        {
            GetCurrentGameObjectSelect();
            DrawMaterialField();
            DrawButtonChangeMaterial();
            DrawCurrentModelImporterSelect();
        }

        private void ChangeMaterial()
        {
            if (m_SelectedObjects == null) return;
            try
            {
                for (int i = 0; i < m_SelectedObjects.Count; i++)
                {
                    ModelImporter modelImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(m_SelectedObjects[i])) as ModelImporter;
                    AssetImporter.SourceAssetIdentifier[] sourceMaterials = typeof(ModelImporter)
                                    .GetProperty("sourceMaterials", BindingFlags.NonPublic | BindingFlags.Instance)?
                                    .GetValue(modelImporter) as AssetImporter.SourceAssetIdentifier[];

                    for (int j = 0; j < sourceMaterials!.Length; j++)
                    {
                        modelImporter!.AddRemap(sourceMaterials[j], m_MaterialDictionary[sourceMaterials[j].name]);
                    }

                    modelImporter!.SaveAndReimport();
                }


                Debug.Log("Change material " + m_SelectedObjects.Count + " models.");
            }
            catch (System.Exception ea)
            {
                Debug.LogError("There was a problem processing the operation.\n" + ea);
            }


        }
        private void DrawButtonChangeMaterial()
        {
            if (GUILayout.Button("Change Material"))
            {
                ChangeMaterial();
            }
        }
        private void GetCurrentGameObjectSelect()
        {
            if (Selection.objects == null) return;
            m_SelectedObjects = Selection.objects.Where(x => x is GameObject).Cast<GameObject>().ToList();
        }
        private void DrawCurrentModelImporterSelect()
        {
            GUILayout.Space(20);
            GUILayout.Label("Current GameObject Select:");

            for (int i = 0; i < m_SelectedObjects.Count; i++)
            {
                GUILayout.Label($"{m_SelectedObjects[i].GetType().Name}: {m_SelectedObjects[i].name}");
            }
        }
        private void DrawMaterialField()
        {
            if (m_SelectedObjects == null) return;
            List<string> materialNameList = new List<string>();
            for (int i = 0; i < m_SelectedObjects.Count; i++)
            {
                ModelImporter modelImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(m_SelectedObjects[i])) as ModelImporter;
                var sourceMaterials = typeof(ModelImporter)
                                .GetProperty("sourceMaterials", BindingFlags.NonPublic | BindingFlags.Instance)?
                                .GetValue(modelImporter) as AssetImporter.SourceAssetIdentifier[];

                for (int j = 0; j < sourceMaterials.Length; j++)
                {

                    if (!m_MaterialDictionary.ContainsKey(sourceMaterials[j].name))
                    {
                        m_MaterialDictionary.Add(sourceMaterials[j].name, null);
                    }
                    if (!materialNameList.Contains(sourceMaterials[j].name))
                    {
                        materialNameList.Add(sourceMaterials[j].name);
                        m_MaterialDictionary[sourceMaterials[j].name] = EditorGUILayout.ObjectField(sourceMaterials[j].name, m_MaterialDictionary[sourceMaterials[j].name], typeof(Material), false) as Material;
                    }
                }
            }
        }


    }
    #endif

}
