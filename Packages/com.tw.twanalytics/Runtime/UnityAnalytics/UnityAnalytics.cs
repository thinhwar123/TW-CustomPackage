using System;
using System.Threading.Tasks;
using TW.DesignPattern;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;


public class UnityAnalytics : MonoBehaviour
{
    public static UnityAnalytics Instance { get; private set; }

    private static readonly Queue<UnityAction> ExecutionEvent = new Queue<UnityAction>();
    private static bool isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private async void Start()
    {
        await InitService();
    }
    private void Update()
    {
        if (!isInitialized) return;
        lock (ExecutionEvent)
        {
            while (ExecutionEvent.Count > 0)
            {
                ExecutionEvent.Dequeue()?.Invoke();
            }
        }
    }
    private async Task InitService()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("UnityServices Initialize: Complete");
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("UnityServices ConsentGiven: Complete");
        isInitialized = true;
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
