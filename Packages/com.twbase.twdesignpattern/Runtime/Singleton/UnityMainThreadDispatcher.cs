using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TW.DesignPattern
{
    public class MainThreadDispatcher : Singleton<MainThreadDispatcher>
	{
        private static readonly Queue<UnityAction> ExecutionQueue = new Queue<UnityAction>();

        public static void Enqueue(UnityAction actionCallback)
        {
            lock (ExecutionQueue)
            {
                ExecutionQueue.Enqueue(actionCallback);
            }
        }
        private void Update()
        {
            lock (ExecutionQueue)
            {
                while (ExecutionQueue.Count > 0)
                {
                    ExecutionQueue.Dequeue()?.Invoke();
                }
            }
        }
    }
}
