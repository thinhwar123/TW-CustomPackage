using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
#endif

namespace TW.Utility.CustomType
{
    [FloatRangeEditor]
    [System.Serializable]
    public struct FloatRange
    {
        public float m_Min;
        public float m_Max;

        public float GetRandomValue()
        {
            if (Mathf.Abs(m_Min - m_Max) < 0.00001f) return m_Min;
            return Mathf.RoundToInt(UnityEngine.Random.Range(m_Min, m_Max) * 10) / 10.0f;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class FloatRangeEditorAttribute : System.Attribute
    {

    }
#if UNITY_EDITOR
    public sealed class FloatRangeEditorAttributeDrawer : OdinAttributeDrawer<FloatRangeEditorAttribute, FloatRange>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            FloatRange value = this.ValueEntry.SmartValue;

            Rect rect = EditorGUILayout.GetControlRect();

            // In Odin, labels are optional and can be null, so we have to account for that.
            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            float prev = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 30;

            value.m_Min = EditorGUI.FloatField(rect.AlignLeft(rect.width * 0.5f - 5), "Min", value.m_Min);
            value.m_Max = EditorGUI.FloatField(rect.AlignRight(rect.width * 0.5f - 5), "Max", value.m_Max);

            EditorGUIUtility.labelWidth = prev;

            this.ValueEntry.SmartValue = value;
        }
    }
#endif
}