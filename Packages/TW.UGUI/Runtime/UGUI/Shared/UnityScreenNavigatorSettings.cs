using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TW.UGUI.AssetLoaders;
using TW.UGUI.Core.Modals;
using UnityEngine;

namespace TW.UGUI.Shared
{
    [CreateAssetMenu(fileName = "UnityScreenNavigatorSettings", menuName = "GlobalConfigs/UnityScreenNavigatorSettings")]
    [GlobalConfig("Assets/Resources/GlobalConfig/")]
    public class UnityScreenNavigatorSettings : GlobalConfig<UnityScreenNavigatorSettings>
    {
        private const string DEFAULT_MODAL_BACKDROP_PREFAB_KEY = "DefaultModalBackdrop";
        

        [SerializeField] private TransitionAnimationObject _screenPushEnterAnimation;

        [SerializeField] private TransitionAnimationObject _screenPushExitAnimation;

        [SerializeField] private TransitionAnimationObject _screenPopEnterAnimation;

        [SerializeField] private TransitionAnimationObject _screenPopExitAnimation;

        [SerializeField] private TransitionAnimationObject _modalEnterAnimation;

        [SerializeField] private TransitionAnimationObject _modalExitAnimation;

        [SerializeField] private TransitionAnimationObject _modalBackdropEnterAnimation;

        [SerializeField] private TransitionAnimationObject _modalBackdropExitAnimation;

        [SerializeField] private TransitionAnimationObject _activityEnterAnimation;

        [SerializeField] private TransitionAnimationObject _activityExitAnimation;

        [SerializeField] private TransitionAnimationObject _sheetEnterAnimation;

        [SerializeField] private TransitionAnimationObject _sheetExitAnimation;

        [SerializeField] private ModalBackdrop _modalBackdropPrefab;

        [SerializeField] private AssetLoaderObject _assetLoader;

        [SerializeField] private bool _enablePooling;

        [SerializeField] private bool _enableInteractionInTransition;

        [SerializeField] private bool _disableModalBackdrop;
        
        [ShowIf("@!_disableModalBackdrop")]
        [SerializeField] private string _modalBackdropResourcePath = DEFAULT_MODAL_BACKDROP_PREFAB_KEY;

        private IAssetLoader _defaultAssetLoader;

        public ITransitionAnimation ScreenPushEnterAnimation => _screenPushEnterAnimation
            ? Instantiate(_screenPushEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(true);

        public ITransitionAnimation ScreenPushExitAnimation => _screenPushExitAnimation
            ? Instantiate(_screenPushExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(false);

        public ITransitionAnimation ScreenPopEnterAnimation => _screenPopEnterAnimation
            ? Instantiate(_screenPopEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(true);

        public ITransitionAnimation ScreenPopExitAnimation => _screenPopExitAnimation
            ? Instantiate(_screenPopExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(false);

        public ITransitionAnimation ModalEnterAnimation => _modalEnterAnimation
            ? Instantiate(_modalEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(true);

        public ITransitionAnimation ModalExitAnimation => _modalExitAnimation
            ? Instantiate(_modalExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(false);

        public ITransitionAnimation ModalBackdropEnterAnimation => _modalBackdropEnterAnimation
            ? Instantiate(_modalBackdropEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(true);

        public ITransitionAnimation ModalBackdropExitAnimation => _modalBackdropExitAnimation
            ? Instantiate(_modalBackdropExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(false);

        public ITransitionAnimation ActivityEnterAnimation => _activityEnterAnimation
            ? Instantiate(_activityEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(true);

        public ITransitionAnimation ActivityExitAnimation => _activityExitAnimation
            ? Instantiate(_activityExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(false);

        public ITransitionAnimation SheetEnterAnimation => _sheetEnterAnimation
            ? Instantiate(_sheetEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(true);

        public ITransitionAnimation SheetExitAnimation => _sheetExitAnimation
            ? Instantiate(_sheetExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(false);

        public string ModalBackdropResourcePath
        {
            get => string.IsNullOrWhiteSpace(_modalBackdropResourcePath)
                ? DEFAULT_MODAL_BACKDROP_PREFAB_KEY
                : _modalBackdropResourcePath;
        }

        public IAssetLoader AssetLoader
        {
            get
            {
                if (_assetLoader)
                {
                    return _assetLoader;
                }

                if (_defaultAssetLoader == null)
                {
#if USN_USE_ADDRESSABLES
                    _defaultAssetLoader = CreateInstance<AddressableAssetLoaderObject>();
#else
                    _defaultAssetLoader = CreateInstance<ResourcesAssetLoaderObject>();
#endif
                }

                return _defaultAssetLoader;
            }
        }

        public bool EnablePooling => _enablePooling;

        public bool EnableInteractionInTransition => _enableInteractionInTransition;

        public bool DisableModalBackdrop => _disableModalBackdrop;


        public ITransitionAnimation GetDefaultScreenTransitionAnimation(bool push, bool enter)
        {
            if (push)
            {
                return enter ? ScreenPushEnterAnimation : ScreenPushExitAnimation;
            }

            return enter ? ScreenPopEnterAnimation : ScreenPopExitAnimation;
        }

        public ITransitionAnimation GetDefaultModalTransitionAnimation(bool enter)
        {
            return enter ? ModalEnterAnimation : ModalExitAnimation;
        }

        public ITransitionAnimation GetDefaultModalBackdropTransitionAnimation(bool enter)
        {
            return enter ? ModalBackdropEnterAnimation : ModalBackdropExitAnimation;
        }

        public ITransitionAnimation GetDefaultActivityTransitionAnimation(bool enter)
        {
            return enter ? ActivityEnterAnimation : ActivityExitAnimation;
        }

        public ITransitionAnimation GetDefaultSheetTransitionAnimation(bool enter)
        {
            return enter ? SheetEnterAnimation : SheetExitAnimation;
        }
    }
}