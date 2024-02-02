using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Sirenix.OdinInspector;

namespace TW.Utility.DesignPattern
{
    [System.Serializable]
    public class StateMachine<T0> where T0 : class 
    {
        [ShowInInspector] private T0 Owner { get; set; }
        [ShowInInspector] private bool IsRunning { get; set; }
        [ShowInInspector] private State<T0> CurrentUniTaskSingletonState { get; set; }
        [ShowInInspector] private Queue<State<T0>> PendingTransitionStateQueue { get; set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }
        public StateMachine(T0 owner)
        {
            Owner = owner;
            PendingTransitionStateQueue = new Queue<State<T0>>();
        }
        
        public void Run()
        {
            CancellationTokenSource = new CancellationTokenSource();
            IsRunning = true;
            _ = ExecuteState();
        }
        
        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }
        
        private async UniTask ExecuteState()
        {
            await foreach (AsyncUnit _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(CancellationTokenSource.Token))
            {
                while (PendingTransitionStateQueue.Count > 0)
                {
                    State<T0> transitionUniTaskState = PendingTransitionStateQueue.Dequeue();
                    await ChangeState(transitionUniTaskState);
                }
                await CurrentUniTaskSingletonState.OnExecute(Owner, CancellationTokenSource.Token);
            }
        }
        
        public void RegisterState(State<T0> singletonState)
        {
            CurrentUniTaskSingletonState = singletonState;
        }
        
        public void RequestTransition(State<T0> singletonState)
        {
            PendingTransitionStateQueue.Enqueue(singletonState);
        }
        
        private async UniTask ChangeState(State<T0> nextSingletonState)
        {
            await CurrentUniTaskSingletonState.OnExit(Owner, CancellationTokenSource.Token);
            CurrentUniTaskSingletonState = nextSingletonState;
            await CurrentUniTaskSingletonState.OnEnter(Owner, CancellationTokenSource.Token);
        }

        public bool IsCurrentState(State<T0> singletonState)
        {
            return CurrentUniTaskSingletonState == singletonState;
        }
        
    }

}