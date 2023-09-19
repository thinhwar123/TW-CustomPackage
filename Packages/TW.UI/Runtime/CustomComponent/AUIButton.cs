using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AUIButton : AUIVisualElement
{
    [field: SerializeField, InlineEditor] public AUIButtonConfig AUIButtonConfig {get; private set;}
    [field: SerializeField] public ACustomButton MainButton {get; private set;}
    public UnityEvent OnClickButton {get; private set;} = new UnityEvent();
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
            if (!MainButton.IsPointerClick) return;
            OnClickButton?.Invoke();
        });
        
        if (AUIButtonConfig != null)
        {
            AUIButtonConfig.SetupSoundEffect(this);
            AUIButtonConfig.SetupAnimEffect(this);
        }
    }

    public bool Interactable
    {
        get => MainButton.interactable;
        set => MainButton.interactable = value;
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

    }
}


