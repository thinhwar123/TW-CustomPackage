using System;
using DG.Tweening;

namespace TW.Utility.Extension
{
    public static class AWaiterExtension
	{
        public static AWaiter DependOn(this AWaiter currentAWaiter, AWaiter dependAWaiter)
        {
            currentAWaiter.Pause();
            dependAWaiter.OnComplete(currentAWaiter.Restart);
            return currentAWaiter;
        }
        public static AWaiter DependOn(this Tween currentSWaiter, AWaiter dependAWaiter)
        {
            AWaiter aWaiter = currentSWaiter;
            return aWaiter.DependOn(dependAWaiter);
        }
    }

    public class AWaiter
    {
        public float Process { get; private set; }
        public float Delay { get; private set; }
        public bool IsComplete { get; private set; }
        public Tween OwnTween { get; set; }
        private Action OnTweenStartCallback { get; set; }
        private Action OnTweenCompleteCallback { get; set; }
        public AWaiter()
        {
            IsComplete = true;
            OnTweenStartCallback += () => IsComplete = false;
            OnTweenCompleteCallback += () => IsComplete = true;
        }
        public AWaiter(Action onStartAction, Action onCompleteAction) : this()
        {
            OnTweenStartCallback += onStartAction;
            OnTweenCompleteCallback += onCompleteAction;
        }
        public AWaiter(Tween tween) : this(() => tween.onPlay?.Invoke(), () => tween.onComplete?.Invoke())
        {
            OwnTween = tween;
            OwnTween.OnUpdate(() => this.Process = OwnTween.Elapsed() / OwnTween.Duration())
                .OnStart(() => OnTweenStartCallback?.Invoke())
                .OnComplete(() => OnTweenCompleteCallback?.Invoke());
        }
        public static implicit operator AWaiter(Tween tween)
        {
            return new AWaiter(tween);
        }
        public void Play()
        {
            Process = 0;
            if (OwnTween == null)
            {
                OwnTween = DOTween.To(() => Process, value => Process = value, 1, Delay)
                    .OnStart(() => OnTweenStartCallback?.Invoke())
                    .OnComplete(() => OnTweenCompleteCallback?.Invoke());
            }
            else
            {
                OwnTween.Restart();
            }
        }
        public void Pause()
        {
            OwnTween?.Pause();
        }
        public void Restart()
        {
            OwnTween?.Restart();
        }
        public void SetDelay(float delay)
        {
            Delay = delay;
        }
        public void OnStart(Action onStartAction)
        {
            OnTweenStartCallback += onStartAction;
        }
        public void OnComplete(Action onCompleteAction)
        {
            OnTweenCompleteCallback += onCompleteAction;
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