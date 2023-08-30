using System.Collections.Generic;
using TW.Utility.DesignPattern;
using UnityEngine.Events;

namespace TW.Utility.Extension
{
    public class AMainThreadDispatcher : Singleton<AMainThreadDispatcher>
	{
        private static readonly Queue<UnityAction> ExecutionQueue = new Queue<UnityAction>();

        public void Enqueue(UnityAction actionCallback)
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
