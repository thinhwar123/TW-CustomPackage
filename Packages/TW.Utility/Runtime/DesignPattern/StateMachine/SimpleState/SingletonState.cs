using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TW.Utility.DesignPattern
{
    public class SingletonState<T0, T1> : State<T0> where T1 : new()
    {
        public interface IHandler
        {
            public UniTask OnStateEnter(CancellationToken token);
            public UniTask OnStateExecute(CancellationToken token);
            public UniTask OnStateExit(CancellationToken token);
        }

        private static T1 instance;
        public static T1 Instance => instance ??= new T1();

        public override async UniTask OnEnter(T0 owner, CancellationToken ct)
        {
            await ((IHandler)owner).OnStateEnter(ct);
        }

        public override async UniTask OnExecute(T0 owner, CancellationToken ct)
        {
            await ((IHandler)owner).OnStateExecute(ct);
        }

        public override async UniTask OnExit(T0 owner, CancellationToken ct)
        {
            await ((IHandler)owner).OnStateExit(ct);
        }
    }
}