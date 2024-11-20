using System;
using System.Collections.Generic;
using TW.Utility.DesignPattern;

namespace TW.Utility.Extension
{
    public class ARealtimeDelay : DDOLSingleton<ARealtimeDelay>
    {
        private List<ARealtimeAwaiter> RealtimeAwaiterList { get; set; } = new();
        private List<ARealtimeAwaiter> TempRealtimeAwaiterList { get; set; } = new();
        public static ARealtimeAwaiter DelayedCall(float time, Action action)
        {
            return new ARealtimeAwaiter(time, action, Instance.RealtimeAwaiterList);
        }

        private void Update()
        {
            DateTime timeNow = DateTime.Now;
            TempRealtimeAwaiterList.Clear();
            foreach (ARealtimeAwaiter aRealtimeAwaiter in RealtimeAwaiterList)
            {
                TempRealtimeAwaiterList.Add(aRealtimeAwaiter);
            }
            
            foreach (ARealtimeAwaiter realtimeAwaiter in TempRealtimeAwaiterList)
            {
                realtimeAwaiter.Update(timeNow);
            }


        }
    }
    
    public class ARealtimeAwaiter
    {
        public List<ARealtimeAwaiter> RealtimeAwaiterList { get; private set; }
        public float Duration { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public Action MainAction { get; private set; }
        public Action CompleteAction { get; private set; }
        public Action UpdateAction { get; private set; }
        public Action StartAction { get; private set; }

        private bool IsStarted { get; set; }
        private bool IsCompleted { get; set; }

        public ARealtimeAwaiter(float time, Action mainAction, List<ARealtimeAwaiter> realtimeAwaiterList)
        {
            Duration = time;
            RealtimeAwaiterList = realtimeAwaiterList;
            MainAction = mainAction;
            StartTime = DateTime.Now;
            EndTime = StartTime.AddSeconds(Duration);

            IsStarted = false;
            IsCompleted = false;

            RealtimeAwaiterList.Add(this);
        }

        public ARealtimeAwaiter OnComplete(Action action)
        {
            CompleteAction = action;
            return this;
        }

        public ARealtimeAwaiter OnUpdate(Action action)
        {
            UpdateAction = action;
            return this;
        }

        public ARealtimeAwaiter OnStart(Action action)
        {
            StartAction = action;
            return this;
        }

        public ARealtimeAwaiter SetDelay(float time)
        {
            StartTime = StartTime.AddSeconds(time);
            EndTime = StartTime.AddSeconds(time);

            return this;
        }

        public ARealtimeAwaiter Kill()
        {
            RealtimeAwaiterList.Remove(this);
            return this;
        }

        internal void Update(DateTime timeNow)
        {
            if (IsCompleted) return;
            if (!IsStarted)
            {
                IsStarted = true;
                StartAction?.Invoke();
            }

            if (timeNow < StartTime) return;
            if (timeNow >= EndTime)
            {
                MainAction?.Invoke();
                CompleteAction?.Invoke();
                IsCompleted = true;
                RealtimeAwaiterList.Remove(this);
            }
            else
            {
                UpdateAction?.Invoke();
            }
        }
    }

}
