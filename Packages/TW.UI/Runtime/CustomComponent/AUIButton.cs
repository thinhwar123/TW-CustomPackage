
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

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            MainButton.OnPointerClickAction.AddListener((eventData) =>
            {
                if (eventData.button != PointerEventData.InputButton.Left) return;
                OnClickButton?.Invoke();
            });
        }
        
    }
}


