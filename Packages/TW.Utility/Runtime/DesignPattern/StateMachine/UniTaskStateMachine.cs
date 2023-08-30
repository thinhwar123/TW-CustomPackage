using System.Linq;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TW.Utility.DesignPattern
{
    [System.Serializable]
    public class UniTaskStateMachine<T> : IStateMachineDebug where T : class
    {
        private readonly T m_Owner;
        private bool m_IsRunning;
        private UniTaskState<T> m_CurrentUniTaskState;
        private readonly Queue<UniTaskState<T>> m_PendingTransitionStateQueue;
        private readonly Queue<UnityAction> m_BeforeTransitionCallbackQueue;
        private readonly Queue<UnityAction> m_AfterTransitionCallbackQueue;
        private CancellationTokenSource m_CancellationTokenSource;
        public UniTaskStateMachine(T owner)
        {
            m_Owner = owner;
            m_PendingTransitionStateQueue = new Queue<UniTaskState<T>>();
            m_BeforeTransitionCallbackQueue = new Queue<UnityAction>();
            m_AfterTransitionCallbackQueue = new Queue<UnityAction>();
        }

        public void Run()
        {
            m_CancellationTokenSource = new CancellationTokenSource();
            m_IsRunning = true;
            _ = ExecuteState();
        }

        public void Stop()
        {
            if (!m_IsRunning) return;
            m_IsRunning = false;
            m_CancellationTokenSource.Cancel();
            m_CancellationTokenSource.Dispose();
        }

        private async UniTask ExecuteState()
        {
            await foreach (AsyncUnit _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(m_CancellationTokenSource.Token))
            {
                while (m_PendingTransitionStateQueue.Count > 0)
                {
                    UniTaskState<T> transitionUniTaskState = m_PendingTransitionStateQueue.Dequeue();
                    await ChangeState(transitionUniTaskState);
                }
                await m_CurrentUniTaskState.OnExecute(m_Owner, m_CancellationTokenSource.Token);
            }

        }


        public void RegisterState(UniTaskState<T> uniTaskState)
        {
            m_CurrentUniTaskState = uniTaskState;
        }

        public void RequestTransition(UniTaskState<T> uniTaskState)
        {
            RequestTransition(uniTaskState, null, null);
        }
        public void RequestTransition(UniTaskState<T> uniTaskState, UnityAction beforeTransitionCallback, UnityAction afterTransitionCallback)
        {
            uniTaskState.OnRequest(m_Owner);
            m_BeforeTransitionCallbackQueue.Enqueue(beforeTransitionCallback);
            m_PendingTransitionStateQueue.Enqueue(uniTaskState);
            m_AfterTransitionCallbackQueue.Enqueue(afterTransitionCallback);
        }

        private async UniTask ChangeState(UniTaskState<T> nextUniTaskState)
        {
            m_BeforeTransitionCallbackQueue.Dequeue()?.Invoke();
            await m_CurrentUniTaskState.OnExit(m_Owner, m_CancellationTokenSource.Token);
            m_CurrentUniTaskState = nextUniTaskState;
            await m_CurrentUniTaskState.OnEnter(m_Owner, m_CancellationTokenSource.Token);
            m_AfterTransitionCallbackQueue.Dequeue()?.Invoke();
        }

        #region Debug

        public bool IsRunning()
        {
            return m_IsRunning;
        }

        public string GetCurrentState()
        {
            return m_CurrentUniTaskState?.ToString();
        }

        public string[] GetPendingTransitionStateQueue()
        {
            return m_PendingTransitionStateQueue?.Select(state => state.ToString()).ToArray();
        } 
        #endregion
    }

}