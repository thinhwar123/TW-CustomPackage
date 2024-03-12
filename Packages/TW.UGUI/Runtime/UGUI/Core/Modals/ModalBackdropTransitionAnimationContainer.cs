using System;
using Sirenix.OdinInspector;
using TW.UGUI.Animation;
using TW.UGUI.Shared;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TW.UGUI.Core.Modals
{
    [Serializable]
    public class ModalBackdropTransitionAnimationContainer
    {
        [SerializeField] private TransitionAnimation _enterAnimation;
        [SerializeField] private TransitionAnimation _exitAnimation;

        public TransitionAnimation EnterAnimation => _enterAnimation;
        public TransitionAnimation ExitAnimation => _exitAnimation;

        public ITransitionAnimation GetAnimation(bool enter)
        {
            var transitionAnimation = enter ? _enterAnimation : _exitAnimation;
            return transitionAnimation.GetAnimation();
        }

        [Serializable]
        public class TransitionAnimation
        {
            [SerializeField] private AnimationAssetType _assetType;
            
            [ShowIf("@_assetType == AnimationAssetType.MonoBehaviour")]
            [SerializeField] private TransitionAnimationBehaviour _animationBehaviour;
            [ShowIf("@_assetType == AnimationAssetType.ScriptableObject")]
            [SerializeField] private TransitionAnimationObject _animationObject;

            public AnimationAssetType AssetType
            {
                get => _assetType;
                set => _assetType = value;
            }

            public TransitionAnimationBehaviour AnimationBehaviour
            {
                get => _animationBehaviour;
                set => _animationBehaviour = value;
            }

            public TransitionAnimationObject AnimationObject
            {
                get => _animationObject;
                set => _animationObject = value;
            }

            public ITransitionAnimation GetAnimation()
            {
                switch (_assetType)
                {
                    case AnimationAssetType.MonoBehaviour:
                        return _animationBehaviour;
                    case AnimationAssetType.ScriptableObject:
                        return Object.Instantiate(_animationObject);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}