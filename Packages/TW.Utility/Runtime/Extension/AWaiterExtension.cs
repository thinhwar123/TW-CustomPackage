using System;
using DG.Tweening;

namespace TW.Utility.Extension
{
    public static class AWaiterExtension
	{
        public static AWaiter DependOn(this AWaiter currentAWaiter, AWaiter dependAWaiter)
        {
            currentAWaiter.OwnTween.Pause();
            dependAWaiter.OwnTween.onComplete += () =>
            {
                currentAWaiter.OwnTween.Restart();
            };
            return currentAWaiter;
        }
        public static AWaiter DependOn(this Tween currentSWaiter, AWaiter dependAWaiter)
        {
            AWaiter aWaiter = currentSWaiter;
            return aWaiter.DependOn(dependAWaiter);
        }
        public static AWaiter DelayCall(this Action action, float time)
        {
            float process = 0; 
            
            return DOTween.To(() => process, value => process = value, 1, time).OnComplete(() => action?.Invoke());
        }

        public static AWaiter DelayCall(this AWaiter aWaiter, float time)
        {
            float process = 0;
            aWaiter.OwnTween = DOTween.To(() => process, value => process = value, 1, time).OnComplete(() => aWaiter.OnTweenCompleteCallback?.Invoke());
            return aWaiter;
        }
    }

    public class AWaiter
    {
        public bool IsComplete { get; private set; }
        public Tween OwnTween { get; set; }
        public Action OnTweenCompleteCallback { get; set; }

        public AWaiter()
        {
            IsComplete = false;
            OnTweenCompleteCallback = null;
        }

        public AWaiter(Action onCompleteAction)
        {
            IsComplete = false;
            OnTweenCompleteCallback = onCompleteAction;
        }
        public AWaiter(Tween tween)
        {
            IsComplete = false;

            OwnTween = tween;
            OwnTween.onComplete += (() =>
            {
                IsComplete = true;
                OnTweenCompleteCallback?.Invoke();
            });

        }
        public static implicit operator AWaiter(Tween tween)
        {
            return new AWaiter(tween);
        }

        public AWaiter OnComplete(Action action)
        {
            OnTweenCompleteCallback = action;
            return this;
        }

        public void Kill()
        {
            OwnTween?.Kill();
        }
        public void InstanceComplete()
        {
            Kill();
            IsComplete = true;
            OnTweenCompleteCallback?.Invoke();
        }
    }
}