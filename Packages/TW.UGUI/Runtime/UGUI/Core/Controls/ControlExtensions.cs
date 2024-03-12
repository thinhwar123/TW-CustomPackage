using System;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Controls
{
    public static class ControlExtensions
    {
        public static void AddLifecycleEvent(
              this Control self
            , Func<Memory<object>, UniTask> initialize = null
            , Func<Memory<object>, UniTask> onWillEnter = null, Action<Memory<object>> onDidEnter = null
            , Func<Memory<object>, UniTask> onWillExit = null, Action<Memory<object>> onDidExit = null
            , Func<Memory<object>, UniTask> onCleanup = null
            , int priority = 0
        )
        {
            var lifecycleEvent = new AnonymousControlLifecycleEvent(
                initialize,
                onWillEnter, onDidEnter,
                onWillExit, onDidExit,
                onCleanup
            );

            self.AddLifecycleEvent(lifecycleEvent, priority);
        }
    }
}