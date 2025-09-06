using System;
using R3;
using UnityEngine.UI;

namespace TW.Reactive.CustomComponent
{
    public static class ReactiveValueExtension
    {
        public static IDisposable CombineLatest<T1, T2>(ReactiveValue<T1> reactiveValue1, ReactiveValue<T2> reactiveValue2, Action<(T1, T2)> onNext)
        {
            return reactiveValue1.ReactiveProperty.CombineLatest(reactiveValue2.ReactiveProperty, (t1, t2) => (t1, t2))
                .Subscribe(onNext);
        }
        private static IDisposable CombineLatest<T1, T2, T3>(ReactiveValue<T1> reactiveValue1, ReactiveValue<T2> reactiveValue2, ReactiveValue<T3> reactiveValue3, Action<(T1, T2, T3)> onNext)
        {
            return reactiveValue1.ReactiveProperty.CombineLatest(reactiveValue2.ReactiveProperty, (t1, t2) => (t1, t2))
                .CombineLatest(reactiveValue3.ReactiveProperty, (t12, t3) => (t12.Item1, t12.Item2, t3))
                .Subscribe(onNext);
        }
    }
}