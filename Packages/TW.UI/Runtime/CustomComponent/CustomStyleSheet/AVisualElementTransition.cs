using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomStyleSheet
{
    [System.Serializable]
    public class AVisualElementTransition
    {
        [field: SerializeField] public TransitionConfig[] TransitionConfigs {get; private set;}
        private Dictionary<string, object> DefaultValues { get; set; } = new Dictionary<string, object>();
        private AProperties TransitionProperties { get; set; } = null;
        public AVisualElementTransition()
        {
            TransitionConfigs = Array.Empty<TransitionConfig>();
        }
        
        public AVisualElementTransition(string transitionValue)
        {
            string[] transitionValues = transitionValue.Split(';');
            TransitionConfigs = transitionValues.Select(x => new TransitionConfig(x)).ToArray();
        }
        private TransitionConfig GetTransitionConfig(string propertyName)
        {
            TransitionConfig transitionConfig = TransitionConfigs.FirstOrDefault(x => x.Property == propertyName);
            if (transitionConfig != null) return transitionConfig;
            transitionConfig = TransitionConfigs.FirstOrDefault(x => x.Property == "all");
            if (transitionConfig != null) return transitionConfig;
            return new TransitionConfig("", 0, Ease.Linear, 0);
        }

        private void TrySaveDefaultValue(string propertyName, object value)
        {
            if (DefaultValues.ContainsKey(propertyName)) return;
            DefaultValues.Add(propertyName, value);
        }
        public List<Tween> PlayTransition(AProperties properties, AVisualElement visualElement)
        {
            List<Tween> tweenList = new List<Tween>();
            if (TransitionConfigs == null) return tweenList;
            TransitionProperties = properties;
            RectTransform rectTransform = visualElement.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = visualElement.GetComponent<CanvasGroup>();
            Image image = visualElement.GetComponent<Image>();
            
            if (properties.TryGetProperty("width", out AProperty width))
            {
                TrySaveDefaultValue("width", rectTransform.rect.width);
                TransitionConfig transitionConfig = GetTransitionConfig("width");
                tweenList.Add(DOTween.To(() => rectTransform.rect.width, 
                    x => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 
                    width.FloatValue, transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }

            if (properties.TryGetProperty("height", out AProperty height))
            {
                TrySaveDefaultValue("height", rectTransform.rect.height);
                TransitionConfig transitionConfig = GetTransitionConfig("height");
                tweenList.Add(DOTween.To(() => rectTransform.rect.height, 
                    x => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x), 
                    height.FloatValue, transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }

            if (properties.TryGetProperty("scale", out AProperty scale))
            {
                TrySaveDefaultValue("scale", rectTransform.localScale);
                TransitionConfig transitionConfig = GetTransitionConfig("scale");
                tweenList.Add(DOTween.To(() => rectTransform.localScale, 
                    x => rectTransform.localScale = x, 
                    new Vector3(scale.Vector2Value.x, scale.Vector2Value.y, 1), transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }

            if (properties.TryGetProperty("background-color", out AProperty backgroundColor))
            {
                TrySaveDefaultValue("background-color", image.color);
                TransitionConfig transitionConfig = GetTransitionConfig("background-color");
                tweenList.Add(DOTween.To(() => image.color, 
                    x => image.color = x, 
                    backgroundColor.ColorValue, transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }
            
            if (properties.TryGetProperty("opacity", out AProperty opacity))
            {
                TrySaveDefaultValue("opacity", canvasGroup.alpha);
                TransitionConfig transitionConfig = GetTransitionConfig("opacity");
                tweenList.Add(DOTween.To(() => canvasGroup.alpha, 
                    x => canvasGroup.alpha = x, 
                    opacity.FloatValue, transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }
            
            return tweenList;
        }
        public List<Tween> PlayTransitionToDefault(AVisualElement visualElement)
        {
            List<Tween> tweenList = new List<Tween>();
            if (TransitionConfigs == null) return tweenList;
            if (TransitionProperties == null) return tweenList;
            AProperties properties = TransitionProperties;
            RectTransform rectTransform = visualElement.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = visualElement.GetComponent<CanvasGroup>();
            Image image = visualElement.GetComponent<Image>();
            
            if (properties.TryGetProperty("width", out AProperty width))
            {
                TransitionConfig transitionConfig = GetTransitionConfig("width");
                tweenList.Add(DOTween.To(() => rectTransform.rect.width, 
                    x => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), 
                    (float)DefaultValues[width.PropertyName], transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }

            if (properties.TryGetProperty("height", out AProperty height))
            {
                TransitionConfig transitionConfig = GetTransitionConfig("height");
                tweenList.Add(DOTween.To(() => rectTransform.rect.height, 
                    x => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x), 
                    (float)DefaultValues[height.PropertyName], transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }

            if (properties.TryGetProperty("scale", out AProperty scale))
            {
                TransitionConfig transitionConfig = GetTransitionConfig("scale");
                tweenList.Add(DOTween.To(() => rectTransform.localScale, 
                    x => rectTransform.localScale = x, 
                    (Vector3)DefaultValues[scale.PropertyName], transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }

            if (properties.TryGetProperty("background-color", out AProperty backgroundColor))
            {
                TransitionConfig transitionConfig = GetTransitionConfig("background-color");
                tweenList.Add(DOTween.To(() => image.color, 
                    x => image.color = x, 
                    (Color)DefaultValues[backgroundColor.PropertyName], transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }
            
            if (properties.TryGetProperty("opacity", out AProperty opacity))
            {
                TransitionConfig transitionConfig = GetTransitionConfig("opacity");
                tweenList.Add(DOTween.To(() => canvasGroup.alpha, 
                    x => canvasGroup.alpha = x, 
                    (float)DefaultValues[opacity.PropertyName], transitionConfig.Duration)
                    .SetEase(transitionConfig.Ease)
                    .SetDelay(transitionConfig.Delay)
                );
            }
            
            TransitionProperties = null;
            return tweenList;
        }
    }

    [System.Serializable]
    public class TransitionConfig
    {
        [field: SerializeField] public string Property {get; private set;}
        [field: SerializeField] public float Duration {get; private set;}
        [field: SerializeField] public Ease Ease {get; private set;}
        [field: SerializeField] public float Delay {get; private set;}

        public TransitionConfig()
        {
            
        }

        public TransitionConfig(string property, float duration, Ease ease, float delay)
        {
            Property = property;
            Duration = duration;
            Ease = ease;
            Delay = delay;
        }
        public TransitionConfig(string transitionValue)
        {
            try
            {
                string[] transitionValues = transitionValue.Split(' ');
                Property = transitionValues[0];
                Duration = float.TryParse(transitionValues[1], out float duration) ? duration : 0f;
                Ease = Enum.TryParse(transitionValues[2], out Ease ease) ? ease : Ease.Linear;
                Delay = float.TryParse(transitionValues[3], out float delay) ? delay : 0f;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e);
            }
        }
    }
}
