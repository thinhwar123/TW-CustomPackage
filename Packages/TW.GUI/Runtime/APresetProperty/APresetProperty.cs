using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TW.GUI
{
    [System.Serializable]
    public class APresetProperty
    {
        public enum Type
        {
            TextValue = 0,
            TextFont = 1,
            TextFontStyle = 2,
            TextFontSize = 3,
            TextAutoSize = 4,
            TextColor = 5,
            TextSpacingOption = 6,
            TextAlignment = 7,
            TextSpriteAsset = 8,
            TextStyleSheetsAsset = 9,
            
            RectPosition = 20,
            RectSize = 21,
            RectPivot = 22,
            RectAnchor = 23,
            RectRotation = 24,
            RectScale = 25,
            
            ImageSprite = 40,
            ImageColor = 41,
            ImageType = 42,
            
        }
#if UNITY_EDITOR
        [field: SerializeField, ReadOnly, HideLabel] public Type PresetPropertyType { get; set; }
        public APresetProperty(Type presetPropertyType)
        {
            PresetPropertyType = presetPropertyType;
        }
        public void Apply(APreset preset, AVisualElement aVisualElement)
        {
            if (Application.isPlaying) return;
            ApplyTextPreset(preset, aVisualElement);
            ApplyRectPreset(preset, aVisualElement);
            ApplyImagePreset(preset, aVisualElement);
        }



        public bool IsOverride(APreset preset, AVisualElement aVisualElement)
        {
            return IsOverrideTextPreset(preset, aVisualElement) ||
                   IsOverrideRectPreset(preset, aVisualElement) ||
                   IsOverrideImagePreset(preset, aVisualElement);
        }

        private bool IsOverrideTextPreset(APreset preset, AVisualElement aVisualElement)
        {
            if (!aVisualElement.TryGetComponent(out TextMeshProUGUI textMeshProUGUI)) return false;
            if (preset is not ATextPreset textPreset) return false;

            switch (PresetPropertyType)
            {
                case Type.TextValue:
                    return textMeshProUGUI.text != textPreset.TextMeshProUGUI.text;
                case Type.TextFont:
                    return textMeshProUGUI.font != textPreset.TextMeshProUGUI.font;
                case Type.TextFontStyle:
                    return textMeshProUGUI.fontStyle != textPreset.TextMeshProUGUI.fontStyle;
                case Type.TextFontSize:
                    return Math.Abs(textMeshProUGUI.fontSize - textPreset.TextMeshProUGUI.fontSize) > 0.01f;
                case Type.TextAutoSize:
                    return textMeshProUGUI.enableAutoSizing != textPreset.TextMeshProUGUI.enableAutoSizing ||
                           Math.Abs(textMeshProUGUI.fontSizeMin - textPreset.TextMeshProUGUI.fontSizeMin) > 0.1f ||
                           Math.Abs(textMeshProUGUI.fontSizeMax - textPreset.TextMeshProUGUI.fontSizeMax) > 0.1f;
                case Type.TextColor:
                    return textMeshProUGUI.color != textPreset.TextMeshProUGUI.color;
                case Type.TextSpacingOption:
                    return Math.Abs(textMeshProUGUI.characterSpacing - textPreset.TextMeshProUGUI.characterSpacing) > 0.01f ||
                           Math.Abs(textMeshProUGUI.wordSpacing - textPreset.TextMeshProUGUI.wordSpacing) > 0.01f ||
                           Math.Abs(textMeshProUGUI.lineSpacing - textPreset.TextMeshProUGUI.lineSpacing) > 0.01f ||
                           Math.Abs(textMeshProUGUI.paragraphSpacing - textPreset.TextMeshProUGUI.paragraphSpacing) > 0.01f;
                case Type.TextAlignment:
                    return textMeshProUGUI.alignment != textPreset.TextMeshProUGUI.alignment;
                case Type.TextSpriteAsset:
                    return textMeshProUGUI.spriteAsset != textPreset.TextMeshProUGUI.spriteAsset;
                case Type.TextStyleSheetsAsset:
                    return textMeshProUGUI.styleSheet != textPreset.TextMeshProUGUI.styleSheet;
            }
            return false;
        }
        
        private bool IsOverrideImagePreset(APreset preset, AVisualElement aVisualElement)
        {
            if (!aVisualElement.TryGetComponent(out Image image)) return false;
            if (preset is not AImagePreset imagePreset) return false;

            switch (PresetPropertyType)
            {
                case Type.ImageSprite:
                    return image.sprite != imagePreset.Image.sprite;
                case Type.ImageColor:
                    return image.color != imagePreset.Image.color;
                case Type.ImageType:
                    return image.type != imagePreset.Image.type &&
                           image.preserveAspect != imagePreset.Image.preserveAspect &&
                           Math.Abs(image.pixelsPerUnitMultiplier - imagePreset.Image.pixelsPerUnitMultiplier) > 0.01f;
            }
            return false;
        }

        private bool IsOverrideRectPreset(APreset preset, AVisualElement aVisualElement)
        {
            if (!aVisualElement.TryGetComponent(out RectTransform rectTransform)) return false;
            if (preset is not ARectPreset rectPreset) return false;
            switch (PresetPropertyType)
            {
                case Type.RectPosition:
                    return rectTransform.anchoredPosition != rectPreset.RectTransform.anchoredPosition;
                case Type.RectSize:
                    return Math.Abs(rectTransform.rect.width - rectPreset.RectTransform.rect.width) > 0.01f ||
                           Math.Abs(rectTransform.rect.height - rectPreset.RectTransform.rect.height) > 0.01f;
                case Type.RectPivot:
                    return rectTransform.pivot != rectPreset.RectTransform.pivot;
                case Type.RectAnchor:
                    return rectTransform.anchorMin != rectPreset.RectTransform.anchorMin ||
                           rectTransform.anchorMax != rectPreset.RectTransform.anchorMax;
                case Type.RectRotation:
                    return rectTransform.localRotation != rectPreset.RectTransform.localRotation;
                case Type.RectScale:
                    return rectTransform.localScale != rectPreset.RectTransform.localScale;
            }
            return false;
        }
        

        private void ApplyTextPreset(APreset preset, AVisualElement aVisualElement)
        {
            if (!aVisualElement.TryGetComponent(out TextMeshProUGUI textMeshProUGUI)) return;
            if (preset is not ATextPreset textPreset) return;
            EditorUtility.SetDirty(textMeshProUGUI);
            switch (PresetPropertyType)
            {
                case Type.TextValue:
                    textMeshProUGUI.text = textPreset.TextMeshProUGUI.text;
                    break;
                case Type.TextFont:
                    textMeshProUGUI.font = textPreset.TextMeshProUGUI.font;
                    break;
                case Type.TextFontStyle:
                    textMeshProUGUI.fontStyle = textPreset.TextMeshProUGUI.fontStyle;
                    break;
                case Type.TextFontSize:
                    textMeshProUGUI.fontSize = textPreset.TextMeshProUGUI.fontSize;
                    break;
                case Type.TextAutoSize:
                    textMeshProUGUI.enableAutoSizing = textPreset.TextMeshProUGUI.enableAutoSizing;
                    textMeshProUGUI.fontSizeMin = textPreset.TextMeshProUGUI.fontSizeMin;
                    textMeshProUGUI.fontSizeMax = textPreset.TextMeshProUGUI.fontSizeMax;
                    break;
                case Type.TextColor:
                    textMeshProUGUI.color = textPreset.TextMeshProUGUI.color;
                    break;
                case Type.TextSpacingOption:
                    textMeshProUGUI.characterSpacing = textPreset.TextMeshProUGUI.characterSpacing;
                    textMeshProUGUI.wordSpacing = textPreset.TextMeshProUGUI.wordSpacing;
                    textMeshProUGUI.lineSpacing = textPreset.TextMeshProUGUI.lineSpacing;
                    textMeshProUGUI.paragraphSpacing = textPreset.TextMeshProUGUI.paragraphSpacing;
                    break;
                case Type.TextAlignment:
                    textMeshProUGUI.alignment = textPreset.TextMeshProUGUI.alignment;
                    break;
                case Type.TextSpriteAsset:
                    textMeshProUGUI.spriteAsset = textPreset.TextMeshProUGUI.spriteAsset;
                    break;
                case Type.TextStyleSheetsAsset:
                    textMeshProUGUI.styleSheet = textPreset.TextMeshProUGUI.styleSheet;
                    break;
            }
        }
        private void ApplyImagePreset(APreset preset, AVisualElement aVisualElement)
        {
            if (!aVisualElement.TryGetComponent(out Image image)) return;
            if (preset is not AImagePreset imagePreset) return;
            EditorUtility.SetDirty(image);
            switch (PresetPropertyType)
            {
                case Type.ImageSprite:
                    image.sprite = imagePreset.Image.sprite;
                    break;
                case Type.ImageColor:
                    image.color = imagePreset.Image.color;
                    break;
                case Type.ImageType:
                    image.type = imagePreset.Image.type;
                    image.preserveAspect = imagePreset.Image.preserveAspect;
                    image.pixelsPerUnitMultiplier = imagePreset.Image.pixelsPerUnitMultiplier;
                    break;
                
            }
        }

        private void ApplyRectPreset(APreset preset, AVisualElement aVisualElement)
        {
            if (!aVisualElement.TryGetComponent(out RectTransform rectTransform)) return;
            if (preset is not ARectPreset rectPreset) return;
            EditorUtility.SetDirty(rectTransform);
            switch (PresetPropertyType)
            {
                case Type.RectPosition:
                    rectTransform.anchoredPosition = rectPreset.RectTransform.anchoredPosition;
                    break;
                case Type.RectSize:
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectPreset.RectTransform.rect.width);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectPreset.RectTransform.rect.height);
                    break;
                case Type.RectPivot:
                    rectTransform.pivot = rectPreset.RectTransform.pivot;
                    break;
                case Type.RectAnchor:
                    rectTransform.anchorMin = rectPreset.RectTransform.anchorMin;
                    rectTransform.anchorMax = rectPreset.RectTransform.anchorMax;
                    break;
                case Type.RectRotation:
                    rectTransform.localRotation = rectPreset.RectTransform.localRotation;
                    break;
                case Type.RectScale:
                    rectTransform.localScale = rectPreset.RectTransform.localScale;
                    break;
            }
        }
#endif
    }
}