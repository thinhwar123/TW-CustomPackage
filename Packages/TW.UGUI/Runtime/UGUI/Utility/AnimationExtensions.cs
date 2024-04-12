using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TW.UGUI.Animation;
using TW.UGUI.Shared;
using UnityEngine;

namespace TW.UGUI.Utility
{
    public static class AnimationExtensions
    {
        // private static Dictionary<IAnimation, AnimationPlayer> AnimationPlayers { get; set; } = new();
        // public static async UniTask PlayAsync(this IAnimation self, IProgress<float> progress = null)
        // {
        //     if (AnimationPlayers.TryGetValue(self, out AnimationPlayer player))
        //     {
        //         player.CancellationTokenSource.Cancel();
        //         player.CancellationTokenSource.Dispose();
        //         
        //         Pool<AnimationPlayer>.Shared.Return(player);
        //         AnimationPlayers.Remove(self);
        //     }
        //     
        //     player = Pool<AnimationPlayer>.Shared.Rent();
        //     AnimationPlayers.Add(self, player);
        //     player.Initialize(self, new CancellationTokenSource());
        //
        //     progress?.Report(0.0f);
        //     player.Play();
        //
        //     while (player.IsFinished == false)
        //     {
        //         await UniTask.NextFrame(player.CancellationTokenSource.Token);
        //         player.Update(Time.unscaledDeltaTime);
        //         progress?.Report(player.Time / (self.Duration + self.Delay));
        //     }
        //
        //     Pool<AnimationPlayer>.Shared.Return(player);
        //     AnimationPlayers.Remove(self);
        // }
        
        // public static async UniTask PlayAsync(this IAnimation self, IProgress<float> progress = null)
        // {
        //     if (AnimationPlayers.TryGetValue(self, out AnimationPlayer player))
        //     {
        //         player.CancellationTokenSource.Cancel();
        //         player.CancellationTokenSource.Dispose();
        //         
        //         Pool<AnimationPlayer>.Shared.Return(player);
        //         AnimationPlayers.Remove(self);
        //     }
        //     
        //     player = Pool<AnimationPlayer>.Shared.Rent();
        //     AnimationPlayers.Add(self, player);
        //     player.Initialize(self, new CancellationTokenSource());
        //
        //     progress?.Report(0.0f);
        //     player.Play();
        //
        //     while (player.IsFinished == false)
        //     {
        //         await UniTask.NextFrame(player.CancellationTokenSource.Token);
        //         player.Update(Time.unscaledDeltaTime);
        //         progress?.Report(player.Time / (self.Duration + self.Delay));
        //     }
        //
        //     Pool<AnimationPlayer>.Shared.Return(player);
        //     AnimationPlayers.Remove(self);
        // }
        
        // public static void Stop(this IAnimation self)
        // {
        //     if (AnimationPlayers.TryGetValue(self, out AnimationPlayer player))
        //     {
        //         player.CancellationTokenSource.Cancel();
        //         player.CancellationTokenSource.Dispose();
        //         
        //         Pool<AnimationPlayer>.Shared.Return(player);
        //         AnimationPlayers.Remove(self);
        //     }
        // }
    }
}