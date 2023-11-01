using System.Collections.Generic;
using DG.Tweening;
using TW.UI.CustomStyleSheet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TW.UI.CustomComponent
{
    public class AUIButton : AVisualElement
    {
        [field: SerializeField] public ACustomButton MainButton { get; private set; }
        [field: SerializeField] public AudioClip ClickSound {get; set;}
        public UnityEvent OnClickButton { get; private set; } = new UnityEvent();
        public List<Tween> AnimTween { get; protected set; } = new List<Tween>();

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
            MainButton.OnPointerDownAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                AnimTween.ForEach(t => t?.Kill());
                AnimTween.Clear();
                AnimTween = ACustomStyleSheet.PlayClickedTransition(this);
            });

            MainButton.OnPointerUpAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                AnimTween.ForEach(t => t?.Kill());
                AnimTween.Clear();
                AnimTween = ACustomStyleSheet.PlayDefaultTransition(this);
            });

            MainButton.OnPointerExitAction.AddListener((eventData) =>
            {
                if (!MainButton.IsPointerDown) return;
                AnimTween.ForEach(t => t?.Kill());
                AnimTween.Clear();
                AnimTween = ACustomStyleSheet.PlayDefaultTransition(this);
            });

            MainButton.OnDestroyButtonAction.AddListener(() =>
            {
                AnimTween.ForEach(t => t?.Kill());
                AnimTween.Clear();
            });
        }
    }
}


