using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.Core.Modals;

namespace TW.UGUI.Core.Modals
{
    public interface IModalLifecycleEventSimple : IModalLifecycleEvent
    {
        UniTask IModalLifecycleEvent.Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        UniTask IModalLifecycleEvent.WillPushEnter(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPushEnter(Memory<object> args)
        {
            
        }

        UniTask IModalLifecycleEvent.WillPushExit(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPushExit(Memory<object> args)
        {
            
        }

        UniTask IModalLifecycleEvent.WillPopEnter(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPopEnter(Memory<object> args)
        {
            
        }

        UniTask IModalLifecycleEvent.WillPopExit(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IModalLifecycleEvent.DidPopExit(Memory<object> args)
        {
            
        }

        UniTask IModalLifecycleEvent.Cleanup(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}