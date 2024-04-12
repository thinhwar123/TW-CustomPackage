using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TW.UGUI.Shared
{
    /// <summary>
    /// Base class for transition animation with MonoBehaviour.
    /// </summary>
    public abstract class TransitionAnimationBehaviour : MonoBehaviour, ITransitionAnimation
    {
        public RectTransform RectTransform { get; private set; }
        public RectTransform PartnerRectTransform { get; private set; }
        
        public abstract float TotalDuration { get; }
        
        void ITransitionAnimation.SetPartner(RectTransform partnerRectTransform)
        {
            PartnerRectTransform = partnerRectTransform;
        }
        void ITransitionAnimation.Setup(RectTransform rectTransform)
        {
            RectTransform = rectTransform;
        }

        public abstract void Setup();

        public abstract UniTask PlayAsync(IProgress<float> progress = null);

        public abstract void Play(IProgress<float> progress = null);

        public abstract void Stop();
    }
}