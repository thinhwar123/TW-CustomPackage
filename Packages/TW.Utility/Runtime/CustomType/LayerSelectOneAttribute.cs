using System;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

namespace TW.Utility.CustomType
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class LayerSelectOneAttribute : Attribute
    {
        
    }
#if UNITY_EDITOR
    public sealed class ColorLayerAttributeDrawer : OdinAttributeDrawer<LayerSelectOneAttribute, int>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Get the current layer value
            int currentLayer = this.ValueEntry.SmartValue;

            // Draw the layer selection dropdown
            EditorGUI.BeginChangeCheck();
            int newLayer = EditorGUILayout.LayerField(label, currentLayer);

            // If the value changed, update it
            if (EditorGUI.EndChangeCheck())
            {
                this.ValueEntry.SmartValue = newLayer;
            }
        }
    }
#endif
}