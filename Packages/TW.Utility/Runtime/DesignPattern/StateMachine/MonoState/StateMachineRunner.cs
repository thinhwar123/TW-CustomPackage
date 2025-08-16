using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DesignPattern.StateMachine.MonoState
{
    public class StateMachineRunner : MonoBehaviour
    {
        private static FastList<StateMachine> stateMachines = new FastList<StateMachine>();
        
#if UNITY_EDITOR
        [ShowInInspector] private StateMachine[] StateMachinesArray => stateMachines.AsArray();
#endif
        public static void Register(StateMachine stateMachine)
        {
            stateMachines.Add(stateMachine);
        }
        public static void Unregister(StateMachine stateMachine)
        {
            stateMachines.Remove(stateMachine);
        }

        private void Update()
        {
            Span<StateMachine> stateMachinesSpan = stateMachines.AsSpan();
            for (int i = 0; i < stateMachinesSpan.Length; i++)
            {
                if (!stateMachinesSpan[i].IsRunning) continue;
                stateMachinesSpan[i].ExecuteState();
            }
        }
    }
}