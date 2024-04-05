using System;
using System.Threading;

namespace TW.UGUI.Animation
{
    internal class AnimationPlayer : IUpdatable
    {
        public IAnimation Animation { get; private set; }

        public bool IsPlaying { get; private set; }

        public float Time { get; private set; }

        public bool IsFinished => Time >= Animation.Duration + Animation.Delay;
        
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public AnimationPlayer()
        {
            
        }
        public void Initialize(IAnimation animation, CancellationTokenSource cancellationTokenSource)
        {
            Animation = animation;
            IsPlaying = default;
            Animation.Setup();
            SetTime(0.0f);
            CancellationTokenSource = cancellationTokenSource;
        }

        public void Update(float deltaTime)
        {
            if (!IsPlaying)
            {
                return;
            }

            SetTime(Time + deltaTime);
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Reset()
        {
            SetTime(0.0f);
        }

        public void SetTime(float time)
        {
            time = Math.Max(0, Math.Min(Animation.Duration + Animation.Delay, time));
            Animation.SetTime(time);

            if (IsPlaying && time >= Animation.Duration + Animation.Delay)
            {
                Stop();
            }

            Time = time;
        }
    }
}