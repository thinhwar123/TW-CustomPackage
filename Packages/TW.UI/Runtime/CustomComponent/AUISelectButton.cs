using System.Collections.Generic;
using DG.Tweening;
using TW.UI.CustomStyleSheet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TW.UI.CustomComponent
{
    public class AUISelectButton : AVisualElement
    {
        [field: SerializeField] public bool IsSelected {get; private set;}
        [field: SerializeField] public ACustomButton MainButton { get; private set; }
        [field: SerializeField] public AudioClip ClickSound {get; set;}
        public UnityEvent OnClickButton { get; private set; } = new UnityEvent();
        public List<Tween> AnimTween { get; private set; } = new List<Tween>();

        protected virtual void Awake()
        {
            Init();
            InitAnim();
        }

        protected virtual void Init()
        {
            MainButton.OnPointerClickAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                OnClickButton?.Invoke();
            });
        }

        protected virtual void InitAnim()
        {
            MainButton.OnPointerClickAction.AddListener((eventData) =>
            {
                if (!IsSelected)
                {
                    SetSelect(true);
                }
            });
            
            MainButton.OnDestroyButtonAction.AddListener(() =>
            {
                AnimTween.ForEach(t => t?.Kill());
                AnimTween.Clear();

            });
        }
        public void SetSelect(bool isSelect)
        {
            IsSelected = isSelect;
            AnimTween.ForEach(t => t?.Kill());
            AnimTween.Clear();
            AnimTween = isSelect ? ACustomStyleSheet.PlaySelectTransition(this) : ACustomStyleSheet.PlayDefaultTransition(this);
        }
    }
}