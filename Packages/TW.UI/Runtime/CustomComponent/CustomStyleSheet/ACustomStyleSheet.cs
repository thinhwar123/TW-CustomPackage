using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomStyleSheet
{
    [System.Serializable]

    public class ACustomStyleSheet
    {
        [field: SerializeField, ASelectorEditor] public ASelector Selector { get; private set; }
        [field: SerializeField, ReadOnly] public AProperties Properties { get; private set; }
        public ACustomStyleSheet()
        {
            
        }
        public ACustomStyleSheet(ASelector selector)
        {
            Selector = selector;
        }
        public void UpdateStyleSheet()
        {
            Properties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector);
        }
        public void ApplyStyleSheet(AVisualElement visualElement)
        {
            if (visualElement.TryGetComponent(out RectTransform rectTransform))
            {
                if (Properties.TryGetProperty("width", out AProperty width))
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width.FloatValue);
                }
                if (Properties.TryGetProperty("height", out AProperty height))
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height.FloatValue);
                }
            }
            if (visualElement.TryGetComponent(out CanvasGroup canvasGroup))
            {
                if (Properties.TryGetProperty("opacity", out AProperty opacity))
                {
                    if (opacity.StringUnit == "%")
                    {
                        canvasGroup.alpha = opacity.FloatValue / 100;
                    }
                    if (opacity.StringUnit == "f")
                    {
                        canvasGroup.alpha = opacity.FloatValue;
                    }
                }

                if (Properties.TryGetProperty("visibility", out AProperty visibility))
                {
                    bool visible = visibility.StringValue == "visible";
                    bool hidden = visibility.StringValue == "hidden";
                    canvasGroup.alpha = visible ? 1 : 0;
                    canvasGroup.interactable = visible;
                    canvasGroup.blocksRaycasts = visible;
                }
            }
            if (visualElement.TryGetComponent(out Image image))
            {
                if (Properties.TryGetProperty("background-color", out AProperty backgroundColor))
                {
                    image.color = backgroundColor.ColorValue;
                }

                if (Properties.TryGetProperty("background-image", out AProperty backgroundImage))
                {
                    image.sprite = backgroundImage.SpriteValue;
                }
            }
            if (visualElement.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
            {
                if (Properties.TryGetProperty("unity-font", out AProperty unityFont))
                {
                    textMeshProUGUI.font = unityFont.FontValue;
                }
                if (Properties.TryGetProperty("unity-font-sprite", out AProperty unityFontSprite))
                {
                    textMeshProUGUI.spriteAsset = unityFontSprite.FontSpriteValue;
                }
                if (Properties.TryGetProperty("unity-font-style", out AProperty unityFontStyle))
                {
                    textMeshProUGUI.styleSheet = unityFontStyle.FontStyleValue;
                }
                if (Properties.TryGetProperty("unity-font-size", out AProperty unityFontSize))
                {
                    textMeshProUGUI.fontSize = unityFontSize.FloatValue;
                }
                if (Properties.TryGetProperty("color", out AProperty unityFontColor))
                {
                    textMeshProUGUI.color = unityFontColor.ColorValue;
                }
            }
        }
    }
}
