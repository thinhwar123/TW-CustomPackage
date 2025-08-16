using System.Threading;
using Cysharp.Threading.Tasks;

namespace TW.Utility.DesignPattern.UniTaskState
{
    public interface IState
    {
        UniTask OnEnter(CancellationToken ct);
        UniTask OnUpdate(CancellationToken ct);
        UniTask OnExit(CancellationToken ct);
    }
}