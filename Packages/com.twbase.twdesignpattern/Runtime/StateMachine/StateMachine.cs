using System.Linq;
using Cysharp.Threading.Tasks.Linq;

namespace TW.DesignPattern
{
    using UnityEngine.Events;
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    [System.Serializable]
    public class StateMachine<T> : IStateMachineDebug where T : class
    {
        private readonly T m_Owner;
        private bool m_IsRunning;
        private State<T> m_CurrentState;
        private readonly Queue<State<T>> m_PendingTransitionStateQueue;
        private readonly Queue<UnityAction> m_BeforeTransitionCallbackQueue;
        private readonly Queue<UnityAction> m_AfterTransitionCallbackQueue;
        private CancellationTokenSource m_CancellationTokenSource;
        public StateMachine(T owner)
        {
            m_Owner = owner;
            m_PendingTransitionStateQueue = new Queue<State<T>>();
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
                    State<T> transitionState = m_PendingTransitionStateQueue.Dequeue();
                    await ChangeState(transitionState);
                }
                await m_CurrentState.OnExecute(m_Owner, m_CancellationTokenSource.Token);
            }

        }


        public void RegisterState(State<T> state)
        {
            m_CurrentState = state;
        }

        public void RequestTransition(State<T> state)
        {
            RequestTransition(state, null, null);
        }
        public void RequestTransition(State<T> state, UnityAction beforeTransitionCallback, UnityAction afterTransitionCallback)
        {
            state.OnRequest(m_Owner);
            m_BeforeTransitionCallbackQueue.Enqueue(beforeTransitionCallback);
            m_PendingTransitionStateQueue.Enqueue(state);
            m_AfterTransitionCallbackQueue.Enqueue(afterTransitionCallback);
        }

        private async UniTask ChangeState(State<T> nextState)
        {
            m_BeforeTransitionCallbackQueue.Dequeue()?.Invoke();
            await m_CurrentState.OnExit(m_Owner, m_CancellationTokenSource.Token);
            m_CurrentState = nextState;
            await m_CurrentState.OnEnter(m_Owner, m_CancellationTokenSource.Token);
            m_AfterTransitionCallbackQueue.Dequeue()?.Invoke();
        }

        #region Debug

        public bool IsRunning()
        {
            return m_IsRunning;
        }

        public string GetCurrentState()
        {
            return m_CurrentState?.ToString();
        }

        public string[] GetPendingTransitionStateQueue()
        {
            return m_PendingTransitionStateQueue?.Select(state => state.ToString()).ToArray();
        } 
        #endregion
    }

}