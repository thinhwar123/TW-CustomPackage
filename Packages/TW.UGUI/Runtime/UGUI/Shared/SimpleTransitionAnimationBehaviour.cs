using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TW.UGUI.Shared
{
    public class SimpleTransitionAnimationBehaviour : TransitionAnimationBehaviour
    {
        public override float TotalDuration => 0;
        public override void Setup()
        {
            
        }

        public override async UniTask PlayAsync(IProgress<float> progress = null)
        {
            
        }

        public override void Play(IProgress<float> progress = null)
        {
            
        }

        public override void Stop()
        {
            
        }
    }
}