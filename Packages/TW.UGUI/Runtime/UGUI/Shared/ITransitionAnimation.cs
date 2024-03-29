﻿using System;
using Cysharp.Threading.Tasks;
using TW.UGUI.Animation;
using UnityEngine;

namespace TW.UGUI.Shared
{
    public interface ITransitionAnimation : IAnimation
    {
        void SetPartner(RectTransform partnerRectTransform);
        
        void Setup(RectTransform rectTransform);
    }

    // internal static class TransitionAnimationExtensions
    // {
    //     public static async UniTask PlayAsync(this ITransitionAnimation self, IProgress<float> progress = null)
    //     {
    //         var player = Pool<AnimationPlayer>.Shared.Rent();
    //         player.Initialize(self);
    //
    //         progress?.Report(0.0f);
    //         player.Play();
    //
    //         while (player.IsFinished == false)
    //         {
    //             await UniTask.NextFrame();
    //             player.Update(Time.unscaledDeltaTime);
    //             progress?.Report(player.Time / self.Duration);
    //         }
    //
    //         Pool<AnimationPlayer>.Shared.Return(player);
    //     }
    // }
}