using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace TW.Utility.DesignPattern
{
    [System.Serializable]
    public class StateMachine : IStateMachineDebug
    {
        public IState CurrentState { get; private set; }
        private Queue<IState> PendingStateQueue { get; set; } = new();
        public bool IsRunning { get; set; } = false;
        private CancellationTokenSource CancellationTokenSource { get; set; }

        public StateMachine()
        {
            IsRunning = false;
        }

        public void Run()
        {
            CancellationTokenSource = new CancellationTokenSource();
            IsRunning = true;
            ExecuteState().Forget();
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }

        private async UniTask ExecuteState()
        {
            UniTaskCancelableAsyncEnumerable<AsyncUnit> asyncEnumerable = UniTaskAsyncEnumerable.EveryUpdate()
                .WithCancellation(CancellationTokenSource.Token);   
            await foreach (AsyncUnit _ in asyncEnumerable)
            {
                while (PendingStateQueue.Count > 0)
                {
                    await ChangeState(PendingStateQueue.Dequeue());
                }

                if (CurrentState != null) await CurrentState.OnUpdate(CancellationTokenSource.Token);
            }
        }

        private async UniTask ChangeState(IState state)
        {
            if (CurrentState != null) await CurrentState.OnExit(CancellationTokenSource.Token);
            CurrentState = state;
            if (CurrentState != null) await CurrentState.OnEnter(CancellationTokenSource.Token);
        }

        public void RegisterState(IState state)
        {
            CurrentState = state;
        }

        public void RequestTransition(IState state)
        {
            PendingStateQueue.Enqueue(state);
        }

        public bool IsCurrentState(IState state)
        {
            return CurrentState == state;
        }

        #region Debug Only

        public bool IsOnRunning()
        {
            return IsRunning;
        }

        public string GetCurrentState()
        {
            return CurrentState?.ToString();
        }

        public string[] GetPendingTransitionStateQueue()
        {
            return PendingStateQueue?.Select(state => state.ToString()).ToArray();
        }

        #endregion
    }
}