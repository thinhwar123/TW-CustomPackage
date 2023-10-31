using System.Collections.Generic;
using DG.Tweening;
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
                TargetSwitchPosition =
                    new Vector3(-TargetSwitchPosition.x, TargetSwitchPosition.y, TargetSwitchPosition.z);
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

}