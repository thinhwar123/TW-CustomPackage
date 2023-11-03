using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
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
            Debug.Log("OnPointerDown");
            base.OnPointerDown(eventData);
            if (!IsInteractable()) return; 
            OnPointerDownAction?.Invoke(eventData);
            IsPointerDown = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
            base.OnPointerUp(eventData);
            if (!IsInteractable()) return; 
            OnPointerUpAction?.Invoke(eventData);
            IsPointerDown = false;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            base.OnPointerEnter(eventData);
            if (!IsInteractable()) return; 
            OnPointerEnterAction?.Invoke(eventData);
            IsPointerEnter = true;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            base.OnPointerExit(eventData);
            if (!IsInteractable()) return; 
            OnPointerExitAction?.Invoke(eventData);
            IsPointerEnter = false;
            IsPointerDown = false;
        }

        public override void OnSelect(BaseEventData eventData)
        {
            Debug.Log("OnSelect");
            base.OnSelect(eventData);
            if (!IsInteractable()) return; 
            OnSelectAction?.Invoke(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            Debug.Log("OnDeselect");
            base.OnDeselect(eventData);
            if (!IsInteractable()) return; 
            OnDeselectAction?.Invoke(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            if (!IsInteractable()) return; 
            OnPointerClickAction?.Invoke(eventData);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDestroyButtonAction?.Invoke();
        }
    }
}