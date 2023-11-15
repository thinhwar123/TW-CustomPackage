using System;
using DG.Tweening;

namespace TW.Utility.Extension
{
    public static class AWaiterExtension
	{
        public static AWaiter DependOn(this AWaiter currentAWaiter, AWaiter dependAWaiter)
        {
            currentAWaiter.Pause();
            dependAWaiter.OnComplete(() => currentAWaiter.Restart());
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
        public float Duration { get; private set; }
        public bool IsComplete { get; private set; }
        public Tween OwnTween { get; set; }
        private Tween HideTween { get; set; }
        private Action OnTweenPlayCallback { get; set; }
        private Action OnTweenCompleteCallback { get; set; }
        public AWaiter()
        {
            IsComplete = true;
            Process = 1;
            Duration = 0;
            OnTweenPlayCallback += () => IsComplete = false;
            OnTweenCompleteCallback += () => IsComplete = true;
        }
        public AWaiter(Action onPlayAction, Action onCompleteAction) : this()
        {
            OnTweenPlayCallback += onPlayAction;
            OnTweenCompleteCallback += onCompleteAction;
        }
        public AWaiter(Tween tween) : this()
        {
            HideTween = tween;
            SetDuration(HideTween.Duration());
            Play();
        }
        public static implicit operator AWaiter(Tween tween)
        {
            return new AWaiter(tween);
        }
        public AWaiter Play()
        {
            Process = 0;
            OwnTween = DOTween.To(() => Process, value => Process = value, 1, Duration)
                .OnStart(() => OnTweenPlayCallback?.Invoke())
                .OnComplete(() => OnTweenCompleteCallback?.Invoke());
            return this;
        }
        public AWaiter Pause()
        {
            OwnTween?.Pause();
            HideTween?.Pause();
            return this;
        }
        public AWaiter Restart()
        {
            OwnTween?.Restart();
            HideTween?.Restart();
            return this;
        }
        public AWaiter Kill()
        {
            OwnTween?.Kill();
            HideTween?.Kill();
            return this;
        }
        public AWaiter SetDuration(float duration)
        {
            Duration = duration;
            return this;
        }
        public AWaiter OnStart(Action onStartAction)
        {
            OnTweenPlayCallback += onStartAction;
            return this;
        }
        public AWaiter OnComplete(Action onCompleteAction)
        {
            OnTweenCompleteCallback += onCompleteAction;
            return this;
        }

        public AWaiter InstanceComplete()
        {
            Kill();
            Process = 1;
            IsComplete = true;
            OnTweenCompleteCallback?.Invoke();

            return this;
        }
    }
}