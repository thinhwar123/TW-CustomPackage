using System;
using R3;
using UnityEngine.UI;

#if REACTIVE_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#endif

namespace TW.Reactive.CustomComponent
{
    public static class ReactiveValueExtension
    {
        public static IDisposable CombineLatest<T1, T2>(ReactiveValue<T1> reactiveValue1, ReactiveValue<T2> reactiveValue2, Action<(T1, T2)> onNext)
        {
            return reactiveValue1.ReactiveProperty.CombineLatest(reactiveValue2.ReactiveProperty, (t1, t2) => (t1, t2))
                .Subscribe(onNext);
        }
        public static IDisposable SetOnClickDestination(this Button self, Action<Unit> onClick)
        {
            return self.onClick
                .AsObservable()
                .Subscribe(onClick)
                .AddTo(self);
        }
#if REACTIVE_UNITASK_SUPPORT
        public static IDisposable SetOnClickDestination(this Button self, Func<UniTask> onClick)
        {
            return self.onClick
                .AsObservable()
                .Subscribe(OnClick)
                .AddTo(self);
            void OnClick(Unit _)
            {
                onClick().Forget();
            }
        }
#endif
    }
}