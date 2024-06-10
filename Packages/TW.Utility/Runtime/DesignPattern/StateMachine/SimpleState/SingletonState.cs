using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TW.Utility.DesignPattern
{
    public class SingletonState<T0, T1> : UniTaskState<T0> where T1 : new() where T0 : class
    {
        public interface IHandler
        {
            public UniTask OnStateEnter(T1 state, CancellationToken token);
            public UniTask OnStateExecute(T1 state,CancellationToken token);
            public UniTask OnStateExit(T1 state, CancellationToken token);
        }

        private static T1 m_Instance;
        public static T1 Instance => m_Instance ??= new T1();

        public override async UniTask OnEnter(T0 owner, CancellationToken ct)
        {
            await ((IHandler)owner).OnStateEnter(Instance, ct);
        }

        public override async UniTask OnExecute(T0 owner, CancellationToken ct)
        {
            await ((IHandler)owner).OnStateExecute(Instance, ct);
        }

        public override async UniTask OnExit(T0 owner, CancellationToken ct)
        {
            await ((IHandler)owner).OnStateExit(Instance, ct);
        }
    }
}