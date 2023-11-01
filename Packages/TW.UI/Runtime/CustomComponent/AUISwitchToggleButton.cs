using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TW.UI.CustomStyleSheet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
{
    public class AUISwitchToggleButton : AVisualElement
    {
        [field: SerializeField] public ACustomButton MainButton { get; private set; }
        [field: SerializeField] public Image ImageSwitch { get; private set; }
        [field: SerializeField] public Image ImageBackground { get; private set; }
        [field: SerializeField] public Color[] ImageSwitchColor {get; private set;}
        [field: SerializeField] public Color[] ImageBackgroundColor {get; private set;}
        [field: SerializeField] public RectTransform Switch { get; private set; }
        [field: SerializeField] public bool Value { get; private set; }

        [field: SerializeField, Unity.Collections.ReadOnly]
        public Vector3 TargetSwitchPosition { get; private set; }

        public UnityEvent<bool> OnClickButton { get; private set; } = new UnityEvent<bool>();
        public List<Tween> AnimTween { get; private set; } = new List<Tween>();
        private bool IsInit { get; set; } = false;
        protected virtual void Awake()
        {
            if (MainButton == null)
            {
                MainButton = GetComponentInChildren<ACustomButton>();
            }
            Init();
        }

        protected virtual void Init()
        {
            if (IsInit) return;
            if (Switch != null)
            {
                TargetSwitchPosition = Switch.localPosition;
            }

            MainButton.OnPointerClickAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                Value = !Value;
                TargetSwitchPosition = new Vector3(-TargetSwitchPosition.x, TargetSwitchPosition.y, TargetSwitchPosition.z);
                AnimTween.ForEach(t => t.Kill());
                AnimTween.Clear();
                AnimTween.Add(Switch.DOLocalMove(TargetSwitchPosition, 0.2f));
                
                AnimTween.Add(ImageSwitch.DOColor(Value ? ImageSwitchColor[0] : ImageSwitchColor[1], 0.2f));
                AnimTween.Add(ImageBackground.DOColor(Value ? ImageBackgroundColor[0] : ImageBackgroundColor[1], 0.2f));
                OnClickButton?.Invoke(Value);
            });
            IsInit = true;
        }

        public void SetupValue(bool value)
        {
            if (value == Value) return;
            if (!IsInit) Init();

            Value = !Value;
            TargetSwitchPosition = new Vector3(-TargetSwitchPosition.x, TargetSwitchPosition.y, TargetSwitchPosition.z);
            Switch.localPosition = TargetSwitchPosition;
        }

    }
    internal static class UITweenHelper
    {
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this Image target, Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(() => target.color, x => target.color = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
    }

}