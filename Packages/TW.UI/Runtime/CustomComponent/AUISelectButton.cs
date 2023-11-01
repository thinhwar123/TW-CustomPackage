using System.Collections.Generic;
using DG.Tweening;
using TW.UI.CustomStyleSheet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TW.UI.CustomComponent
{
    public class AUISelectButton : AUIButton
    {
        [field: SerializeField] public bool IsSelected {get; private set;}

        protected override void InitAnim()
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