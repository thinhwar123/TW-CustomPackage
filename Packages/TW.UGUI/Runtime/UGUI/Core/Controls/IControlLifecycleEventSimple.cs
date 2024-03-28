using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Controls
{
    public interface IControlLifecycleEventSimple : IControlLifecycleEvent
    {
        UniTask IControlLifecycleEvent.Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        UniTask IControlLifecycleEvent.WillEnter(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IControlLifecycleEvent.DidEnter(Memory<object> args)
        {
            
        }

        UniTask IControlLifecycleEvent.WillExit(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IControlLifecycleEvent.DidExit(Memory<object> args)
        {
            
        }

        UniTask IControlLifecycleEvent.Cleanup(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}