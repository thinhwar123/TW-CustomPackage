using System.Threading.Tasks;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;

namespace TW.Analytics
{
    public class UnityAnalytics : MonoBehaviour
    {
        public static UnityAnalytics Instance { get; private set; }

        private static readonly Queue<UnityAction> ExecutionEvent = new Queue<UnityAction>();
        private static bool m_IsInitialized = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this.gameObject);
            }
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private async void Start()
        {
            await InitService();
        }
        private void Update()
        {
            if (!m_IsInitialized) return;
            lock (ExecutionEvent)
            {
                while (ExecutionEvent.Count > 0)
                {
                    ExecutionEvent.Dequeue()?.Invoke();
                }
            }
        }
        private static async Task InitService()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("UnityServices Initialize: Complete");
#if UNITY_ANALYTICS_5
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("UnityServices ConsentGiven: Complete"); 
#endif
            m_IsInitialized = true;
        }
        public static void SendCustomEvent(string eventName, IDictionary<string, object> eventParams)
        {
            lock (ExecutionEvent)
            {
                ExecutionEvent.Enqueue(() =>
                {
                    AnalyticsService.Instance.CustomData(eventName, eventParams);
                });
            }
        }
    }

}