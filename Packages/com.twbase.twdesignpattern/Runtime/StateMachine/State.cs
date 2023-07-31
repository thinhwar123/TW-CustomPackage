using System.Threading;

namespace TW.DesignPattern
{
    using Cysharp.Threading.Tasks;

    public class State<T> where T : class
    {
        public virtual void OnRequest(T owner)
        {

        }
        public virtual async UniTask OnEnter(T owner, CancellationToken ct)
        {
            await UniTask.Yield();
        }

        public virtual async UniTask OnExecute(T owner, CancellationToken ct)
        {
            await UniTask.Yield();
        }

        public virtual async UniTask OnExit(T owner, CancellationToken ct)
        {
            await UniTask.Yield();
        }

    }

}