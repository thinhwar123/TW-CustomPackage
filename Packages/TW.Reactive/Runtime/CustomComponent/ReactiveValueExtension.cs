using System;
using R3;
using UnityEngine;

namespace TW.Reactive.CustomComponent
{
    public static class ReactiveValueExtension
    {
        public static IDisposable CombineLatest<T1, T2>(ReactiveValue<T1> reactiveValue1, ReactiveValue<T2> reactiveValue2, Action<(T1, T2)> onNext)
        {
            return reactiveValue1.ReactiveProperty.CombineLatest(reactiveValue2.ReactiveProperty, (t1, t2) => (t1, t2))
                .Subscribe(onNext);
        }

    }
}