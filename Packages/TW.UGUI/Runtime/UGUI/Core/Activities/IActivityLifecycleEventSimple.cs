using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Activities
{
    public interface IActivityLifecycleEventSimple : IActivityLifecycleEvent
    {

        UniTask IActivityLifecycleEvent.Initialize(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
        
        UniTask IActivityLifecycleEvent.WillEnter(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }

        void IActivityLifecycleEvent.DidEnter(Memory<object> args)
        {
            
        }

        UniTask IActivityLifecycleEvent.WillExit(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
        
        void IActivityLifecycleEvent.DidExit(Memory<object> args)
        {
            
        }

        UniTask IActivityLifecycleEvent.Cleanup(Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}