using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AUISwitchToggleButton : AUIVisualElement
{
    [field: SerializeField, InlineEditor] public AUISwitchToggleButtonConfig AUISwitchToggleButtonConfig {get; private set;}
    [field: SerializeField] public ACustomButton MainButton {get; private set;}
    [field: SerializeField] public Image ImageSwitch {get; private set;}
    [field: SerializeField] public Image ImageBackground {get; private set;}
    [field: SerializeField] public RectTransform Switch {get; private set;}
    [field: SerializeField] public bool Value {get; private set;}
    [field: SerializeField, Unity.Collections.ReadOnly] public Vector3 TargetSwitchPosition {get; private set;}
    public UnityEvent<bool> OnClickButton {get; private set;} = new UnityEvent<bool>();
    public List<Tween> AnimTween { get; private set; } = new List<Tween>();
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    protected virtual void Init()
    {
        MainButton.OnPointerUpAction.AddListener((eventData) =>
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            Value = !Value;
            TargetSwitchPosition = new Vector3(-TargetSwitchPosition.x, TargetSwitchPosition.y, TargetSwitchPosition.z);
            if (!MainButton.IsPointerClick) return;
            OnClickButton?.Invoke(Value);
        });
        if (AUISwitchToggleButtonConfig != null)
        {
            AUISwitchToggleButtonConfig.SetupSoundEffect(this);
            AUISwitchToggleButtonConfig.SetupAnimEffect(this);
        }
    }
    protected override void Setup()
    {
        if (MainButton == null)
        {
            MainButton = GetComponentInChildren<ACustomButton>();
        }
    }

    protected override void Config()
    {
        if (Switch != null)
        {
            TargetSwitchPosition = Switch.localPosition;
        }

    }

    public void SetupValue(bool value)
    {
        if (value == Value) return; 
        
        Value = !Value;
        TargetSwitchPosition = new Vector3(-TargetSwitchPosition.x, TargetSwitchPosition.y, TargetSwitchPosition.z);
        if (AUISwitchToggleButtonConfig != null)
        {
            ImageBackground.color = Value ? 
                AUISwitchToggleButtonConfig.ActiveBackgroundColor : 
                AUISwitchToggleButtonConfig.DeActiveBackgroundColor;
            ImageSwitch.color = Value ? 
                AUISwitchToggleButtonConfig.ActiveSwitchColor : 
                AUISwitchToggleButtonConfig.DeActiveSwitchColor;
        }
    }
}
