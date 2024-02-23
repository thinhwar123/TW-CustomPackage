using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TW.GUI
{
    public class ACustomButton : Button
    {
        private Transform m_Transform;
        public Transform Transform => m_Transform = m_Transform != null ? m_Transform : transform;
        public bool IsPointerDown { get; private set; }
        public bool IsPointerEnter { get; private set; }
        public UnityEvent<PointerEventData> OnPointerDownAction { get; set; } = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnPointerUpAction { get; set; } = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnPointerEnterAction { get; set; } = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnPointerExitAction { get; set; } = new UnityEvent<PointerEventData>();
        public UnityEvent<BaseEventData> OnSelectAction { get; set; } = new UnityEvent<BaseEventData>();
        public UnityEvent<BaseEventData> OnDeselectAction { get; set; } = new UnityEvent<BaseEventData>();
        public UnityEvent<PointerEventData> OnPointerClickAction { get; set; } = new UnityEvent<PointerEventData>();
        public UnityEvent OnDestroyButtonAction { get; set; } = new UnityEvent();

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!IsInteractable()) return; 
            OnPointerDownAction?.Invoke(eventData);
            IsPointerDown = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!IsInteractable()) return; 
            OnPointerUpAction?.Invoke(eventData);
            IsPointerDown = false;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!IsInteractable()) return; 
            OnPointerEnterAction?.Invoke(eventData);
            IsPointerEnter = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (!IsInteractable()) return; 
            OnPointerExitAction?.Invoke(eventData);
            IsPointerEnter = false;
            IsPointerDown = false;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            if (!IsInteractable()) return; 
            OnSelectAction?.Invoke(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            if (!IsInteractable()) return; 
            OnDeselectAction?.Invoke(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable()) return; 
            OnPointerClickAction?.Invoke(eventData);
            onClick?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDestroyButtonAction?.Invoke();
        }
    }

}