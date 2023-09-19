using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class AUISwitchTab : AUIVisualElement
{
    [field: SerializeField] public Button[] Buttons {get; private set;}
    [field: SerializeField] public RectTransform ImageSelect {get; private set;}
    private Tween MoveTween { get; set; }
    protected override void Setup()
    {
        Buttons = GetComponentsInChildren<Button>();
    }

    protected override void Config()
    {
        Buttons.ForEach(b => b.onClick.AddListener(() => OnSelectButton(b)));
        // set size of image select to size of first button
        ImageSelect.sizeDelta = new Vector2(Buttons[0].GetComponent<RectTransform>().sizeDelta.x, ImageSelect.sizeDelta.y);
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
