using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using TW.UGUI.Animation;
using TW.UGUI.Shared;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TW.UGUI.Core.Controls
{
    [Serializable]
    public class ControlTransitionAnimationContainer
    {
        [SerializeField] private List<TransitionAnimation> _enterAnimations = new();
        [SerializeField] private List<TransitionAnimation> _exitAnimations = new();

        public List<TransitionAnimation> EnterAnimations => _enterAnimations;
        public List<TransitionAnimation> ExitAnimations => _exitAnimations;

        public ITransitionAnimation GetAnimation(bool enter, string partnerTransitionIdentifier)
        {
            var anims = enter ? _enterAnimations : _exitAnimations;
            var anim = anims.FirstOrDefault(x => x.IsValid(partnerTransitionIdentifier));
            var result = anim?.GetAnimation();
            return result;
        }

        [Serializable]
        public class TransitionAnimation
        {
            [SerializeField] private string _partnerControlIdentifierRegex;

            [SerializeField] private AnimationAssetType _assetType;

            [ShowIf("@_assetType == AnimationAssetType.MonoBehaviour")]
            [SerializeField] private TransitionAnimationBehaviour _animationBehaviour;

            [ShowIf("@_assetType == AnimationAssetType.ScriptableObject")]
            [SerializeField] private TransitionAnimationObject _animationObject;

            private Regex _partnerControlIdentifierRegexCache;

            public string PartnerControlIdentifierRegex
            {
                get => _partnerControlIdentifierRegex;
                set => _partnerControlIdentifierRegex = value;
            }

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

            public bool IsValid(string partnerControlIdentifier)
            {
                if (GetAnimation() == null)
                {
                    return false;
                }

                // If the partner identifier is not registered, the animation is always valid.
                if (string.IsNullOrEmpty(_partnerControlIdentifierRegex))
                {
                    return true;
                }
                
                if (string.IsNullOrEmpty(partnerControlIdentifier))
                {
                    return false;
                }

                if (_partnerControlIdentifierRegexCache == null)
                {
                    _partnerControlIdentifierRegexCache = new Regex(_partnerControlIdentifierRegex);
                }

                return _partnerControlIdentifierRegexCache.IsMatch(partnerControlIdentifier);
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