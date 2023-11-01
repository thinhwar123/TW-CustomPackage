using DG.Tweening;
using Sirenix.Utilities;
using TW.UI.CustomStyleSheet;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UI.CustomComponent
{
    public class AUISwitchTab : AVisualElement
    {
        [field: SerializeField] public Button[] Buttons { get; private set; }
        [field: SerializeField] public RectTransform ImageSelect { get; private set; }
        private Tween MoveTween { get; set; }

        protected virtual void Awake()
        {
            Buttons = GetComponentsInChildren<Button>();
            Buttons.ForEach(b => b.onClick.AddListener(() => OnSelectButton(b)));
            // set size of image select to size of first button
            ImageSelect.sizeDelta =
                new Vector2(Buttons[0].GetComponent<RectTransform>().sizeDelta.x, ImageSelect.sizeDelta.y);
        }
        protected virtual void OnSelectButton(Button button)
        {
            MoveTween?.Kill();
            MoveTween = ImageSelect.DOMoveX(button.transform.position.x, 0.2f);
        }

        public void SetSelectButton(int index)
        {
            MoveTween?.Kill();
            MoveTween = ImageSelect.DOMoveX(Buttons[index].transform.position.x, 0.2f);
        }
    }

}