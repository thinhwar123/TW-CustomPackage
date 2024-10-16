using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.UGUI.Shared
{
    public sealed class SortingLayerSelectAttribute : PropertyAttribute
    {
        
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SortingLayerSelectAttribute))]
    public class SortingLayerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.BeginProperty(position, label, property);

                string[] sortingLayerNames = new string[SortingLayer.layers.Length];
                int[] sortingLayerIDs = new int[SortingLayer.layers.Length];
                for (int i = 0; i < SortingLayer.layers.Length; i++)
                {
                    sortingLayerNames[i] = SortingLayer.layers[i].name;
                    sortingLayerIDs[i] = SortingLayer.layers[i].id;
                }

                int currentLayerID = property.intValue;
                int currentIndex = Array.IndexOf(sortingLayerIDs, currentLayerID);
                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, sortingLayerNames);

                if (newIndex != currentIndex)
                {
                    property.intValue = sortingLayerIDs[newIndex];
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use SortingLayer with int.");
            }
        }
    }
#endif
}