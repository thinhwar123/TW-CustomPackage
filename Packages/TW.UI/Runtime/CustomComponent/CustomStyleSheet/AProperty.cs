using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.Utilities;
using TMPro;
using TW.Utility.Extension;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
#endif

namespace TW.UI.CustomStyleSheet
{
    [System.Serializable]
    public class AProperty
    {
        [field: SerializeField] public string PropertyName {get; set;}
        [field: SerializeField] public string StringValue {get; set;}
        [field: SerializeField] public float FloatValue {get; set;}
        [field: SerializeField] public Sprite SpriteValue {get; set;}
        [field: SerializeField] public Color ColorValue {get; set;}
        [field: SerializeField] public TMP_FontAsset FontValue {get; set;}
        [field: SerializeField] public TMP_SpriteAsset FontSpriteValue {get; set;}
        [field: SerializeField] public TMP_StyleSheet FontStyleValue {get; set;}
        [field: SerializeField] public (float, float) Vector2Value {get; set;}
        [field: SerializeField] public AudioClip AudioValue {get; set;}
        [field: SerializeField] public string StringUnit {get; set;}
        
        public string GetUSSPropertyValue()
        {
            APropertyConfig aPropertyConfig = APropertyGlobalConfig.Instance.PropertyConfigs.FirstOrDefault(x => x.PropertyName == PropertyName);
            if (aPropertyConfig == null) return string.Empty;
            switch (aPropertyConfig.PropertyValueType)
            {
                case APropertyConfig.EPropertyValueType.String:
                    return StringValue;
                case APropertyConfig.EPropertyValueType.Float:
                    return FloatValue.ToString(CultureInfo.InvariantCulture);
                case APropertyConfig.EPropertyValueType.Sprite:
                    return SpriteValue == null ? string.Empty : SpriteValue.name;
                case APropertyConfig.EPropertyValueType.Color:
                    return ColorValue.ToHex();
                case APropertyConfig.EPropertyValueType.Font:
                    return FontValue == null ? string.Empty : FontValue.name;
                case APropertyConfig.EPropertyValueType.FontSprite:
                    return FontSpriteValue == null ? string.Empty : FontSpriteValue.name;
                case APropertyConfig.EPropertyValueType.FontStyle:
                    return FontStyleValue == null ? string.Empty : FontStyleValue.name;
                case APropertyConfig.EPropertyValueType.Vector2:
                    return $"({Vector2Value.Item1.ToString(CultureInfo.InvariantCulture)}, {Vector2Value.Item2.ToString(CultureInfo.InvariantCulture)})";
                case APropertyConfig.EPropertyValueType.Audio:
                    return AudioValue == null ? string.Empty : AudioValue.name;
                case APropertyConfig.EPropertyValueType.Special:
                    return string.Empty;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public bool IsAutoProperty()
        {
            return StringUnit == "auto";
        }

        public AProperty()
        {
            
        }
        public AProperty(AProperty other)
        {
            PropertyName = other.PropertyName;
            StringValue = other.StringValue;
            FloatValue = other.FloatValue;
            SpriteValue = other.SpriteValue;
            ColorValue = other.ColorValue;
            FontValue = other.FontValue;
            FontSpriteValue = other.FontSpriteValue;
            FontStyleValue = other.FontStyleValue;
            Vector2Value = other.Vector2Value;
            AudioValue = other.AudioValue;
            StringUnit = other.StringUnit;
        }
        public bool IsMatch(AProperty other)
        {
            return PropertyName == other.PropertyName &&
                   StringValue == other.StringValue &&
                   Math.Abs(FloatValue - other.FloatValue) < 0.01f &&
                   SpriteValue == other.SpriteValue &&
                   ColorValue == other.ColorValue &&
                   FontValue == other.FontValue &&
                   FontSpriteValue == other.FontSpriteValue &&
                   FontStyleValue == other.FontStyleValue &&
                   Vector2Value == other.Vector2Value &&
                   AudioValue == other.AudioValue &&
                   StringUnit == other.StringUnit;
        }
    }
    
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true)]
    public sealed class APropertyEditorAttribute : System.Attribute
    {

    }
#if UNITY_EDITOR
    public sealed class APropertyEditorAttributeDrawer : OdinAttributeDrawer<APropertyEditorAttribute, AProperty>
    {
        private AProperty ConfigValue { get; set; }
        private int TypeChoiceIndex { get; set;}
        private int UnitChoiceIndex { get; set;}
        private int SpecialChoiceIndex { get; set;}
        private string[] PropertiesName { get; set;}
        private string[] FormatPropertiesName { get; set;}
        private APropertyConfig APropertyConfig { get; set;}
        private bool HasPropertyUnit => !APropertyConfig.Unit.IsNullOrWhitespace();
        private string[] PropertyUnit => HasPropertyUnit ? APropertyConfig.Unit.Replace(" ", "").Split('|') : Array.Empty<string>();
        private string[] SpecialOptions => APropertyConfig.SpecialOptions;
        protected override void Initialize()
        {
            base.Initialize();
            ConfigValue = this.ValueEntry.SmartValue;
            PropertiesName = APropertyGlobalConfig.Instance.PropertyConfigs.Select(x => x.PropertyName).ToArray();


            FormatPropertiesName = PropertiesName.Select(FormatPropertyType).ToArray();
        }
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            DrawPropertyType(ConfigValue, rect, 0.32f);
            DrawPropertyValue(ConfigValue, rect, 0.66f, 0.1f, 0.02f);

            if (this.ValueEntry.SmartValue.IsMatch(ConfigValue)) return;
            this.ValueEntry.WeakValues.ForceMarkDirty();
            this.ValueEntry.SmartValue = new AProperty(ConfigValue);
        }
        private void DrawPropertyType(AProperty value, Rect fullRect, float rectWidth)
        {
            using (new GUIColorScope(new Color(0.42f, 0.58f, 0.91f)))
            {            
                TypeChoiceIndex = System.Array.IndexOf(PropertiesName, value.PropertyName);
                TypeChoiceIndex = EditorGUI.Popup(fullRect.AlignLeft(fullRect.width * rectWidth), TypeChoiceIndex < 0 ? 0 : TypeChoiceIndex, FormatPropertiesName);
                value.PropertyName = PropertiesName[TypeChoiceIndex];
            }
        }
        private void DrawPropertyValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            APropertyConfig = APropertyGlobalConfig.Instance.PropertyConfigs[TypeChoiceIndex];
            switch (APropertyConfig.PropertyValueType)
            {
                case APropertyConfig.EPropertyValueType.String:
                    DrawStringValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Float:
                    DrawFloatValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Sprite:
                    DrawSpriteValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Color:
                    DrawColorValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Font:
                    DrawFontValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.FontSprite:
                    DrawFontSpriteValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.FontStyle:
                    DrawFontStyleValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Vector2:
                    DrawVector2Value(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Audio:
                    DrawAudioValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                case APropertyConfig.EPropertyValueType.Special:
                    DrawSpecialValue(value, fullRect, rectValueWidth, rectUnitWidth, space);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void DrawStringValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.StringValue = EditorGUI.TextField(rectValue, value.StringValue);
            }
            else
            {
                value.StringValue = "";
            }

        }
        private void DrawFloatValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.FloatValue = EditorGUI.FloatField(rectValue, value.FloatValue);
            }
            else
            {
                value.FloatValue = 0;
            }
        }
        private void DrawSpriteValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.SpriteValue = EditorGUI.ObjectField(rectValue, value.SpriteValue, typeof(Sprite), false) as Sprite;
            }
            else
            {
                value.SpriteValue = null;
            }
        }
        private void DrawColorValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.ColorValue = EditorGUI.ColorField(rectValue, value.ColorValue);
            }
            else
            {
                value.ColorValue = Color.white;
            }
        }
        private void DrawFontValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.FontValue = EditorGUI.ObjectField(rectValue, value.FontValue, typeof(TMP_FontAsset), false) as TMP_FontAsset;
            }
            else
            {
                value.FontValue = null;
            }

        }
        private void DrawFontSpriteValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.FontSpriteValue = EditorGUI.ObjectField(rectValue, value.FontSpriteValue, typeof(TMP_SpriteAsset), false) as TMP_SpriteAsset;
            }
            else
            {
                value.FontSpriteValue = null;
            }
        }
        private void DrawFontStyleValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.FontStyleValue = EditorGUI.ObjectField(rectValue, value.FontStyleValue, typeof(TMP_StyleSheet), false) as TMP_StyleSheet;
            }
            else
            {
                value.FontStyleValue = null;
            }
        }
        private void DrawVector2Value(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);
            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto") 
            {
                Vector2 vector2Value = EditorGUI.Vector2Field(rectValue, "" ,new Vector2(value.Vector2Value.Item1, value.Vector2Value.Item2));
                value.Vector2Value = (vector2Value.x, vector2Value.y);
            }
            else
            {
                value.Vector2Value = (0, 0);
            }
        }
        private void DrawAudioValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);

            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto")
            {
                value.AudioValue = EditorGUI.ObjectField(rectValue, value.AudioValue, typeof(AudioClip), false) as AudioClip;
            }
            else
            {
                value.AudioValue = null;
            }
        }
        private void DrawSpecialValue(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space)
        {
            Rect rectValue = fullRect.AlignRight(fullRect.width * rectValueWidth);
            DrawPropertyUnit(value, fullRect, rectValueWidth, rectUnitWidth, space, ref rectValue);
            if (!HasPropertyUnit || UnitChoiceIndex < 0 || PropertyUnit[UnitChoiceIndex] != "auto") 
            {
                SpecialChoiceIndex = System.Array.IndexOf(APropertyConfig.SpecialOptions, value.StringValue);
                SpecialChoiceIndex = EditorGUI.Popup(rectValue, SpecialChoiceIndex < 0 ? 0 : SpecialChoiceIndex, APropertyConfig.SpecialOptions);
                value.StringValue = APropertyConfig.SpecialOptions[SpecialChoiceIndex];
            }
            else
            {
                value.StringValue = APropertyConfig.SpecialOptions[0];
            }
        }
        private void DrawPropertyUnit(AProperty value, Rect fullRect, float rectValueWidth, float rectUnitWidth, float space, ref Rect rectValue)
        {
            if (HasPropertyUnit)
            {
                Rect rectUnit = fullRect.AlignRight(fullRect.width * rectUnitWidth);
                UnitChoiceIndex = System.Array.IndexOf(PropertyUnit, value.StringUnit);
                if (UnitChoiceIndex >= 0 && PropertyUnit[UnitChoiceIndex] == "auto")
                    rectUnit = fullRect.AlignRight(fullRect.width * rectValueWidth);
                UnitChoiceIndex = EditorGUI.Popup(rectUnit, UnitChoiceIndex < 0 ? 0 : UnitChoiceIndex, PropertyUnit);
                value.StringUnit = PropertyUnit[UnitChoiceIndex];

                rectValue = rectValue.SubXMax(fullRect.width * (rectUnitWidth + space));
            }
            else
            {
                value.StringUnit = string.Empty;
            }
        }
        private string FormatPropertyType(string propertyType)
        {
            // convert string like "unity-font-sprite" to "Unity Font Sprite" by replace "-" with " " and capitalize first letter
            return Regex.Replace(propertyType, @"(^\w)|(-\w)", m => m.Value.Replace("-", " ").ToUpper());
        }
    }
#endif
}