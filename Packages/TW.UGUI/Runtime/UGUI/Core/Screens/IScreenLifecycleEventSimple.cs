using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Screens
{
    public interface IScreenLifecycleEventSimple : IScreenLifecycleEvent
    {
        UniTask IScreenLifecycleEvent.Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        UniTask IScreenLifecycleEvent.WillPushEnter(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPushEnter(Memory<object> args)
        {
            
        }

        UniTask IScreenLifecycleEvent.WillPushExit(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPushExit(Memory<object> args)
        {
            
        }

        UniTask IScreenLifecycleEvent.WillPopEnter(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPopEnter(Memory<object> args)
        {
            
        }

        UniTask IScreenLifecycleEvent.WillPopExit(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IScreenLifecycleEvent.DidPopExit(Memory<object> args)
        {
            
        }

        UniTask IScreenLifecycleEvent.Cleanup(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}