using System.Threading;
using Cysharp.Threading.Tasks;

namespace TW.Utility.DesignPattern
{
    public class State<T>
    {
        public virtual UniTask OnEnter(T owner, CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnExecute(T owner, CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnExit(T owner, CancellationToken ct)
        {
            return UniTask.CompletedTask;
        }
    }
}