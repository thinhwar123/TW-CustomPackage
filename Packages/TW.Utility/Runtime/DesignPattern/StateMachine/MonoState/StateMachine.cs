using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace DesignPattern.StateMachine.MonoState
{
    [System.Serializable]
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        private Queue<IState> PendingStateQueue { get; set; } = new();
        public bool IsRunning { get; set; } = false;

        public StateMachine()
        {
            IsRunning = false;
        }

        public void Run()
        {
            if (IsRunning) return;
            IsRunning = true;
            StateMachineRunner.Register(this);
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            StateMachineRunner.Unregister(this);
        }

        public void ExecuteState()
        {
            while (PendingStateQueue.Count > 0)
            {
                ChangeState(PendingStateQueue.Dequeue());
            }

            CurrentState?.OnUpdate();
        }

        private void ChangeState(IState state)
        {
            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState?.OnEnter();
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

#if UNITY_EDITOR
        [ShowInInspector] private bool DebugIsRunning => IsOnRunning();
        [ShowInInspector] private string DebugCurrentState => GetCurrentState();
        [ShowInInspector] private string[] DebugPendingStateQueue => GetPendingTransitionStateQueue();

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
#endif

        #endregion
    }
}