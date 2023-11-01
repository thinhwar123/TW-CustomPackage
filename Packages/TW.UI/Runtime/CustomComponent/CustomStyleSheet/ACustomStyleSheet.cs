using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using TW.UI.CustomComponent;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomStyleSheet
{
    [System.Serializable]

    public class ACustomStyleSheet
    {

        [field: SerializeField, ASelectorEditor] public ASelector Selector { get; private set; }
        [field: SerializeField, ReadOnly,ShowIf("@Selector.StateSelector == \"Default\"")] public AProperties DefaultProperties { get; private set; }
        [field: SerializeField, ReadOnly, ShowIf("@Selector.StateSelector == \"Open\"")] public AProperties OpenProperties { get; private set; }
        [field: SerializeField, ReadOnly, ShowIf("@Selector.StateSelector == \"Close\"")] public AProperties CloseProperties { get; private set; }
        [field: SerializeField, ReadOnly, ShowIf("@Selector.StateSelector == \"Clicked\"")] public AProperties ClickedProperties { get; private set; }
        [field: SerializeField, ReadOnly, ShowIf("@Selector.StateSelector == \"Select\"")] public AProperties SelectProperties { get; private set; }
        [field: SerializeField, ReadOnly, ShowIf("@Selector.StateSelector == \"Active\"")] public AProperties ActiveProperties { get; private set; }
        [field: SerializeField, ReadOnly, ShowIf("@Selector.StateSelector == \"Inactive\"")] public AProperties InactiveProperties { get; private set; }
        [field: SerializeField] public AVisualElementTransition VisualElementTransition {get; set;}
        public ACustomStyleSheet()
        {
            
        }
        public ACustomStyleSheet(ASelector selector)
        {
            Selector = selector;
        }
        public void UpdateStyleSheet()
        {
            DefaultProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Default);
            OpenProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Open);
            CloseProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Close);
            ClickedProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Clicked);
            SelectProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Selected);
            ActiveProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Active);
            InactiveProperties = AStyleSheetGlobalConfig.Instance.GetProperties(Selector, AVisualElement.State.Inactive);

            if (DefaultProperties.TryGetProperty("transition", out AProperty transition))
            {
                VisualElementTransition = new AVisualElementTransition(transition.StringValue);
            }
            else
            {
                VisualElementTransition = new AVisualElementTransition();
            }
        }
        public void ApplyStyleSheet(AVisualElement visualElement)
        {
            if (visualElement.TryGetComponent(out RectTransform rectTransform))
            {
                if (DefaultProperties.TryGetProperty("width", out AProperty width))
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width.FloatValue);
                }
                if (DefaultProperties.TryGetProperty("height", out AProperty height))
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height.FloatValue);
                }
            }
            if (visualElement.TryGetComponent(out CanvasGroup canvasGroup))
            {
                if (DefaultProperties.TryGetProperty("opacity", out AProperty opacity))
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

                if (DefaultProperties.TryGetProperty("visibility", out AProperty visibility))
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
                if (DefaultProperties.TryGetProperty("background-color", out AProperty backgroundColor))
                {
                    image.color = backgroundColor.ColorValue;
                }

                if (DefaultProperties.TryGetProperty("background-image", out AProperty backgroundImage))
                {
                    image.sprite = backgroundImage.SpriteValue;
                }
            }
            if (visualElement.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
            {
                if (DefaultProperties.TryGetProperty("unity-font", out AProperty unityFont))
                {
                    textMeshProUGUI.font = unityFont.FontValue;
                }
                if (DefaultProperties.TryGetProperty("unity-font-sprite", out AProperty unityFontSprite))
                {
                    textMeshProUGUI.spriteAsset = unityFontSprite.FontSpriteValue;
                }
                if (DefaultProperties.TryGetProperty("unity-font-style", out AProperty unityFontStyle))
                {
                    textMeshProUGUI.styleSheet = unityFontStyle.FontStyleValue;
                }
                if (DefaultProperties.TryGetProperty("unity-font-size", out AProperty unityFontSize))
                {
                    textMeshProUGUI.fontSize = unityFontSize.FloatValue;
                }
                if (DefaultProperties.TryGetProperty("color", out AProperty unityFontColor))
                {
                    textMeshProUGUI.color = unityFontColor.ColorValue;
                }
            }   
            if (visualElement.TryGetComponent(out AUIButton button))
            {
                if (DefaultProperties.TryGetProperty("background-color", out AProperty backgroundColor))
                {
                    button.MainButton.GetComponent<Image>().color = backgroundColor.ColorValue;
                }

                if (DefaultProperties.TryGetProperty("background-image", out AProperty backgroundImage))
                {
                    button.MainButton.GetComponent<Image>().sprite = backgroundImage.SpriteValue;
                }

                if (DefaultProperties.TryGetProperty("audio", out AProperty audio))
                {
                    button.ClickSound= audio.AudioValue;
                }
            }
        }
        public List<Tween> PlayDefaultTransition(AVisualElement visualElement)
        {
            return VisualElementTransition.PlayTransitionToDefault(visualElement);
        }
        public List<Tween> PlayOpenTransition(AVisualElement visualElement)
        {
            if (OpenProperties == null || OpenProperties.Count == 0) return new List<Tween>();
            return VisualElementTransition.PlayTransition(OpenProperties, visualElement);
        }
        public List<Tween> PlayCloseTransition(AVisualElement visualElement)
        {
            if (CloseProperties == null || CloseProperties.Count == 0) return new List<Tween>();
            return VisualElementTransition.PlayTransition(CloseProperties, visualElement);
        }
        public List<Tween> PlayClickedTransition(AVisualElement visualElement)
        {
            if (ClickedProperties == null || ClickedProperties.Count == 0) return new List<Tween>();
            return VisualElementTransition.PlayTransition(ClickedProperties, visualElement);
        }
        public List<Tween> PlaySelectTransition(AVisualElement visualElement)
        {
            if (SelectProperties == null || SelectProperties.Count == 0) return new List<Tween>();
            return VisualElementTransition.PlayTransition(SelectProperties, visualElement);
        }
        public List<Tween> PlayActiveTransition(AVisualElement visualElement)
        {
            if (ActiveProperties == null || ActiveProperties.Count == 0) return new List<Tween>();
            return VisualElementTransition.PlayTransition(ActiveProperties, visualElement);
        }
        public List<Tween> PlayInactiveTransition(AVisualElement visualElement)
        {
            if (InactiveProperties == null || InactiveProperties.Count == 0) return new List<Tween>();
            return VisualElementTransition.PlayTransition(InactiveProperties, visualElement);
        }
    }
}
