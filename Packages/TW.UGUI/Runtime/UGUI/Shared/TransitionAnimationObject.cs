﻿using UnityEngine;

namespace TW.UGUI.Shared
{
    public abstract class TransitionAnimationObject : ScriptableObject, ITransitionAnimation
    {
        public RectTransform RectTransform { get; private set; }
        public RectTransform PartnerRectTransform { get; private set; }
        public abstract float Delay { get; }
        public abstract float Duration { get; }

        void ITransitionAnimation.SetPartner(RectTransform partnerRectTransform)
        {
            PartnerRectTransform = partnerRectTransform;
        }

        void ITransitionAnimation.Setup(RectTransform rectTransform)
        {
            RectTransform = rectTransform;
        }

        public abstract void SetTime(float time);

        public abstract void Setup();
    }
}