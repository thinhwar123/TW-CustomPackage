using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TW.UGUI.Shared
{
    [CreateAssetMenu(fileName = "SimpleTransitionAnimation", menuName = "Screen Navigator/Animations/Simple Transition Animation")]
    public class SimpleTransitionAnimationObject : TransitionAnimationObject
    {
        [field: SerializeField] public bool ActiveTarget {get; set;}
        public override float TotalDuration => 0;
        public static SimpleTransitionAnimationObject CreateInstance(bool activeTarget)
        {
            SimpleTransitionAnimationObject anim = CreateInstance<SimpleTransitionAnimationObject>();
            anim.ActiveTarget = activeTarget;
            return anim;
        }
        public override void Setup()
        {
            
        }

        public override UniTask PlayAsync(IProgress<float> progress = null)
        {
            RectTransform.gameObject.SetActive(ActiveTarget);
            return UniTask.CompletedTask;
        }

        public override void Play(IProgress<float> progress = null)
        {
            PlayAsync(progress).Forget();
        }

        public override void Stop()
        {
            
        }
    }
}