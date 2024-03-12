
using TW.UGUI.Core.Views;
using TW.UGUI.Shared;

namespace TW.UGUI.Core.Modals
{
    public readonly struct ModalOptions
    {
        public readonly float? backdropAlpha;
        public readonly bool? closeWhenClickOnBackdrop;
        public readonly string modalBackdropResourcePath;
        public readonly ViewOptions options;

        public ModalOptions(
              in ViewOptions options
            , in float? backdropAlpha = null
            , in bool? closeWhenClickOnBackdrop = null
            , string modalBackdropResourcePath = null
        )
        {
            this.options = options;
            this.backdropAlpha = backdropAlpha;
            this.closeWhenClickOnBackdrop = closeWhenClickOnBackdrop;
            this.modalBackdropResourcePath = modalBackdropResourcePath;
        }

        public ModalOptions(
              string resourcePath
            , bool playAnimation = true
            , OnViewLoadedCallback onLoaded = null
            , bool loadAsync = true
            , in float? backdropAlpha = null
            , in bool? closeWhenClickOnBackdrop = null
            , string modalBackdropResourcePath = null
            , PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings
        )
        {
            this.options = new(resourcePath, playAnimation, onLoaded, loadAsync, poolingPolicy);
            this.backdropAlpha = backdropAlpha;
            this.closeWhenClickOnBackdrop = closeWhenClickOnBackdrop;
            this.modalBackdropResourcePath = modalBackdropResourcePath;
        }

        public static implicit operator ModalOptions(in ViewOptions options)
            => new(options);

        public static implicit operator ModalOptions(string resourcePath)
            => new(new ViewOptions(resourcePath));

        public static implicit operator ViewOptions(in ModalOptions options)
            => options.options;
    }
}
