using TW.UGUI.Core.Views;
using TW.UGUI.Shared;

namespace TW.UGUI.Core.Screens
{
    public readonly struct ScreenOptions
    {
        public readonly bool stack;
        public readonly ViewOptions options;

        public ScreenOptions(
              in ViewOptions options
            , bool stack = true
        )
        {
            this.options = options;
            this.stack = stack;
        }

        public ScreenOptions(
              string resourcePath
            , bool playAnimation = true
            , OnViewLoadedCallback onLoaded = null
            , bool loadAsync = true
            , bool stack = true
            , PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings
        )
        {
            this.options = new(resourcePath, playAnimation, onLoaded, loadAsync, poolingPolicy);
            this.stack = stack;
        }

        public static implicit operator ScreenOptions(in ViewOptions options)
            => new(options);

        public static implicit operator ScreenOptions(string resourcePath)
            => new(new ViewOptions(resourcePath));

        public static implicit operator ViewOptions(in ScreenOptions options)
            => options.options;
    }
}
