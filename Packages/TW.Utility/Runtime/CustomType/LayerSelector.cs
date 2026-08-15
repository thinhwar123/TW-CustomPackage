using System;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

namespace TW.Utility.CustomType
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class LayerSelector : Attribute
    {
    }

#if UNITY_EDITOR
    public sealed class LayerSelectorAttributeDrawer : OdinAttributeDrawer<LayerSelector, int>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            ValueEntry.SmartValue = EditorGUI.LayerField(rect, label, ValueEntry.SmartValue);
        }
    }
#endif
}