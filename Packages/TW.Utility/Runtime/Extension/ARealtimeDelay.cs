using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TW.Utility.Extension
{
    public class ARealtimeDelay : MonoBehaviour
    {
        private static ARealtimeDelay m_Instance;

        private static ARealtimeDelay Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    // Find singleton
                    m_Instance = FindObjectOfType<ARealtimeDelay>();

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<ARealtimeDelay>();
                        singletonObject.name = typeof(ARealtimeDelay).ToString() + " (Singleton)";
                    }

                }

                return m_Instance;
            }
        }

        [field: SerializeField] public List<ARealtimeAwaiter> RealtimeAwaiterList { get; private set; } = new();
        private List<ARealtimeAwaiter> RemoveRealtimeAwaiterList { get; set; } = new();
        public static ARealtimeAwaiter DelayedCall(float time, Action action)
        {
            return new ARealtimeAwaiter(time, action, Instance.RealtimeAwaiterList, Instance.RemoveRealtimeAwaiterList);
        }

        private void Update()
        {
            DateTime timeNow = DateTime.Now;
            foreach (ARealtimeAwaiter realtimeAwaiter in RealtimeAwaiterList)
            {
                realtimeAwaiter.Update(timeNow);
            }

            foreach (ARealtimeAwaiter removeARealtime in RemoveRealtimeAwaiterList)
            {
                RealtimeAwaiterList.Remove(removeARealtime);
            }
            RemoveRealtimeAwaiterList.Clear();
        }
    }

    [Serializable]
    public class ARealtimeAwaiter
    {
        public List<ARealtimeAwaiter> RealtimeAwaiterList { get; private set; }
        public List<ARealtimeAwaiter> RemoveRealtimeAwaiterList { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public Action MainAction { get; private set; }
        public Action CompleteAction { get; private set; }
        public Action UpdateAction { get; private set; }
        public Action StartAction { get; private set; }

        private bool IsStarted { get; set; }
        private bool IsCompleted { get; set; }

        public ARealtimeAwaiter(float time, Action mainAction, List<ARealtimeAwaiter> realtimeAwaiterList, List<ARealtimeAwaiter> removeRealtimeAwaiterList)
        {
            Duration = time;
            RealtimeAwaiterList = realtimeAwaiterList;
            RemoveRealtimeAwaiterList = removeRealtimeAwaiterList;
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
            RemoveRealtimeAwaiterList.Add(this);
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
                RemoveRealtimeAwaiterList.Add(this);
            }
            else
            {
                UpdateAction?.Invoke();
            }
        }
    }

}