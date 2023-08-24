using System;
using DG.Tweening;

namespace TW.Utility
{
    public static class SWaiterExtension
	{
        public static SWaiter DependOn(this SWaiter currentSWaiter, SWaiter dependSWaiter)
        {
            currentSWaiter.OwnTween.Pause();
            dependSWaiter.OwnTween.onComplete += () =>
            {
                currentSWaiter.OwnTween.Restart();
            };
            return currentSWaiter;
        }
        public static SWaiter DelayCall(this Action action, float time)
        {
            float process = 0; 
            return DOTween.To(() => process, value => process = value, 1, time).OnComplete(() => action?.Invoke());
        }
    }

    public class SWaiter
    {
        public Tween OwnTween { get; private set; }
        public bool IsComplete { get; private set; }
        private Action OnTweenCompleteCallback { get; set; }
        public SWaiter(Tween tween)
        {
            IsComplete = false;

            OwnTween = tween;
            OwnTween.onComplete += (() =>
            {
                IsComplete = true;
                OnTweenCompleteCallback?.Invoke();
            });

        }
        public static implicit operator SWaiter(Tween tween)
        {
            return new SWaiter(tween);
        }

        public SWaiter OnComplete(Action action)
        {
            OnTweenCompleteCallback = action;
            return this;
        }
    }
}