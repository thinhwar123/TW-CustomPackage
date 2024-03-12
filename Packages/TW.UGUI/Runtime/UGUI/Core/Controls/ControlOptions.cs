using System;
using TW.UGUI.Core.Views;
using TW.UGUI.Shared;

namespace TW.UGUI.Core.Controls
{
    public delegate void OnControlLoadedCallback(int controlId, Control control, Memory<object> args);

    public readonly struct ControlOptions
    {
        public readonly bool loadAsync;
        public readonly bool playAnimation;
        public readonly string resourcePath;
        public readonly PoolingPolicy poolingPolicy;
        public readonly OnControlLoadedCallback onLoaded;

        public ControlOptions(
              string resourcePath
            , bool playAnimation = true
            , OnControlLoadedCallback onLoaded = null
            , bool loadAsync = true
            , PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings
        )
        {
            this.loadAsync = loadAsync;
            this.playAnimation = playAnimation;
            this.resourcePath = resourcePath;
            this.onLoaded = onLoaded;
            this.poolingPolicy = poolingPolicy;
        }

        public ViewOptions AsViewOptions()
            => new(resourcePath, playAnimation, null, loadAsync, poolingPolicy);

        public static implicit operator ControlOptions(string resourcePath)
            => new(resourcePath);
    }
}