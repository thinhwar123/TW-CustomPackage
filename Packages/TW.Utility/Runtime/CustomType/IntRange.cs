using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
#endif

namespace TW.Utility.CustomType
{
    [IntRangeEditor]
    [System.Serializable]
    public struct IntRange
    {
        public int m_Min;
        public int m_Max;

        public int GetRandomValue()
        {
            return m_Min == m_Max ? m_Min : Random.Range(m_Min, m_Max + 1);
        }
    }


    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class IntRangeEditorAttribute : System.Attribute
    {

    }
#if UNITY_EDITOR
    public sealed class IntRangeEditorAttributeDrawer : OdinAttributeDrawer<IntRangeEditorAttribute, IntRange>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            IntRange value = this.ValueEntry.SmartValue;

            Rect rect = EditorGUILayout.GetControlRect();

            // In Odin, labels are optional and can be null, so we have to account for that.
            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            float prev = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 30;

            value.m_Min = EditorGUI.IntField(rect.AlignLeft(rect.width * 0.5f - 5), "Min", value.m_Min);
            value.m_Max = EditorGUI.IntField(rect.AlignRight(rect.width * 0.5f - 5), "Max", value.m_Max);

            EditorGUIUtility.labelWidth = prev;

            this.ValueEntry.SmartValue = value;
        }
    }
#endif

}