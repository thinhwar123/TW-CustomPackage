using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Activities
{
    public class AnonymousActivityWindowLifecycleEvent : IActivityLifecycleEvent
    {
        /// <see cref="IActivityLifecycleEvent.DidEnter(Memory{object})"/>
        public event Action<Memory<object>> OnDidEnter;

        /// <see cref="IActivityLifecycleEvent.DidExit(Memory{object})"/>
        public event Action<Memory<object>> OnDidExit;

        public AnonymousActivityWindowLifecycleEvent(
              Func<Memory<object>, UniTask> initialize = null
            , Func<Memory<object>, UniTask> onWillEnter = null, Action<Memory<object>> onDidEnter = null
            , Func<Memory<object>, UniTask> onWillExit = null, Action<Memory<object>> onDidExit = null
            , Func<Memory<object>, UniTask> onCleanup = null
        )
        {
            if (initialize != null)
                OnInitialize.Add(initialize);

            if (onWillEnter != null)
                OnWillEnter.Add(onWillEnter);

            OnDidEnter = onDidEnter;

            if (onWillExit != null)
                OnWillExit.Add(onWillExit);

            OnDidExit = onDidExit;

            if (onCleanup != null)
                OnCleanup.Add(onCleanup);
        }

        /// <see cref="IActivityLifecycleEvent.Initialize(Memory{object})"/>
        public List<Func<Memory<object>, UniTask>> OnInitialize { get; } = new();

        /// <see cref="IActivityLifecycleEvent.WillEnter(Memory{object})"/>
        public List<Func<Memory<object>, UniTask>> OnWillEnter { get; } = new();

        /// <see cref="IActivityLifecycleEvent.WillExit(Memory{object})"/>
        public List<Func<Memory<object>, UniTask>> OnWillExit { get; } = new();

        /// <see cref="IActivityLifecycleEvent.Cleanup"/>
        public List<Func<Memory<object>, UniTask>> OnCleanup { get; } = new();

        async UniTask IActivityLifecycleEvent.Initialize(Memory<object> args)
        {
            foreach (var onInitialize in OnInitialize)
                await onInitialize.Invoke(args);
        }

        async UniTask IActivityLifecycleEvent.WillEnter(Memory<object> args)
        {
            foreach (var onWillShowEnter in OnWillEnter)
                await onWillShowEnter.Invoke(args);
        }

        void IActivityLifecycleEvent.DidEnter(Memory<object> args)
        {
            OnDidEnter?.Invoke(args);
        }

        async UniTask IActivityLifecycleEvent.WillExit(Memory<object> args)
        {
            foreach (var onWillHideEnter in OnWillExit)
                await onWillHideEnter.Invoke(args);
        }

        void IActivityLifecycleEvent.DidExit(Memory<object> args)
        {
            OnDidExit?.Invoke(args);
        }

        async UniTask IActivityLifecycleEvent.Cleanup(Memory<object> args)
        {
            foreach (var onCleanup in OnCleanup)
                await onCleanup.Invoke(args);
        }
    }
}