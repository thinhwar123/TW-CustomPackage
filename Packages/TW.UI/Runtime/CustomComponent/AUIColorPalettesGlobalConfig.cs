using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace TW.UI.CustomComponent
{
    [CreateAssetMenu(fileName = "AUIColorPalletGlobalConfig", menuName = "GlobalConfigs/AUIColorPalettesGlobalConfig")] 
    [GlobalConfig("Assets/Resources/GlobalConfig/")]
    public class AUIColorPalettesGlobalConfig : GlobalConfig<AUIColorPalettesGlobalConfig>
    {
        [Serializable]
        public class ColorPalette
        {
            public string m_PaletteName;
            [ListDrawerSettings(CustomAddFunction = nameof(CustomAddColor))]
            public Color[] m_PaletteColor;
            
            public Color CustomAddColor()
            {
                return Color.white;
            }
        }
        [field: ColorPaletteEditor]
        [field: SerializeField] public ColorPalette[] ColorPalettes { get; private set; }
    }
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ColorPaletteEditorAttribute : System.Attribute
    {

    }
#if UNITY_EDITOR
    public sealed class ColorPaletteAttributeDrawer : OdinAttributeDrawer<ColorPaletteEditorAttribute, AUIColorPalettesGlobalConfig.ColorPalette>
    {
        private bool IsEditing { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            IsEditing = false;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            if (IsEditing)
            {
                this.ValueEntry.SmartValue.m_PaletteName = EditorGUI.TextField(rect.AlignLeft(rect.width - 60), "Palette Name", this.ValueEntry.SmartValue.m_PaletteName);
            }
            else
            {
                
                EditorGUI.DrawRect(rect.AlignLeft(rect.width - 60), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                EditorGUI.LabelField(rect.AlignLeft(rect.width - 60), "Palette Name", this.ValueEntry.SmartValue.m_PaletteName);
            }
            if (GUI.Button(rect.AlignRight(50), IsEditing ? "Save" : "Edit"))
            {
                IsEditing = !IsEditing;
            }
            if (IsEditing)
            {
                this.Property.Children[1].Draw();
            }
            else
            {
                // draw color range
                AUIColorPalettesGlobalConfig.ColorPalette colorPalette = this.ValueEntry.SmartValue;
                Color[] colorArray = colorPalette.m_PaletteColor;
                Rect colorRectFull = EditorGUILayout.GetControlRect();
                float colorWidth = colorRectFull.width / colorArray.Length;
                for (int i = 0; i < colorArray.Length; i++)
                {
                    EditorGUI.DrawRect(colorRectFull.AlignRight(colorRectFull.width - colorWidth * i), colorArray[i]);
                }
            }


            
        }
    }   
#endif
}

