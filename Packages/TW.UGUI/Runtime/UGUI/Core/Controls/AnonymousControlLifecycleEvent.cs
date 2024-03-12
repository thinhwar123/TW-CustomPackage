using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TW.UGUI.Core.Controls
{
    public class AnonymousControlLifecycleEvent : IControlLifecycleEvent
    {
        /// <see cref="IControlLifecycleEvent.DidEnter(Memory{object})"/>
        public event Action<Memory<object>> OnDidEnter;

        /// <see cref="IControlLifecycleEvent.DidExit(Memory{object})"/>
        public event Action<Memory<object>> OnDidExit;

        public AnonymousControlLifecycleEvent(
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

        /// <see cref="IControlLifecycleEvent.Initialize(Memory{object})"/>
        public List<Func<Memory<object>, UniTask>> OnInitialize { get; } = new();

        /// <see cref="IControlLifecycleEvent.WillEnter(Memory{object})"/>
        public List<Func<Memory<object>, UniTask>> OnWillEnter { get; } = new();

        /// <see cref="IControlLifecycleEvent.WillExit(Memory{object})"/>
        public List<Func<Memory<object>, UniTask>> OnWillExit { get; } = new();

        /// <see cref="IControlLifecycleEvent.Cleanup"/>
        public List<Func<Memory<object>, UniTask>> OnCleanup { get; } = new();

        async UniTask IControlLifecycleEvent.Initialize(Memory<object> args)
        {
            foreach (var onInitialize in OnInitialize)
                await onInitialize.Invoke(args);
        }

        async UniTask IControlLifecycleEvent.WillEnter(Memory<object> args)
        {
            foreach (var onWillEnter in OnWillEnter)
                await onWillEnter.Invoke(args);
        }

        void IControlLifecycleEvent.DidEnter(Memory<object> args)
        {
            OnDidEnter?.Invoke(args);
        }

        async UniTask IControlLifecycleEvent.WillExit(Memory<object> args)
        {
            foreach (var onWillExit in OnWillExit)
                await onWillExit.Invoke(args);
        }

        void IControlLifecycleEvent.DidExit(Memory<object> args)
        {
            OnDidExit?.Invoke(args);
        }

        async UniTask IControlLifecycleEvent.Cleanup(Memory<object> args)
        {
            foreach (var onCleanup in OnCleanup)
                await onCleanup.Invoke(args);
        }
    }
}