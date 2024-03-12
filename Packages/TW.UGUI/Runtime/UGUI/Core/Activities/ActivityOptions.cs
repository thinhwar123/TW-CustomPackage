using TW.UGUI.Core.Views;
using TW.UGUI.Shared;

namespace TW.UGUI.Core.Activities
{
    public readonly struct ActivityOptions
    {
        public readonly SortingLayerId? sortingLayer;
        public readonly int? orderInLayer;
        public readonly ViewOptions options;

        public ActivityOptions(
              in ViewOptions options
            , in SortingLayerId? sortingLayer = null
            , in int? orderInLayer = null
        )
        {
            this.options = options;
            this.sortingLayer = sortingLayer;
            this.orderInLayer = orderInLayer;
        }

        public ActivityOptions(
              string resourcePath
            , bool playAnimation = true
            , OnViewLoadedCallback onLoaded = null
            , bool loadAsync = true
            , in SortingLayerId? sortingLayer = null
            , in int? orderInLayer = null
            , PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings
        )
        {
            this.options = new(resourcePath, playAnimation, onLoaded, loadAsync, poolingPolicy);
            this.sortingLayer = sortingLayer;
            this.orderInLayer = orderInLayer;
        }

        public static implicit operator ActivityOptions(in ViewOptions options)
            => new(options);

        public static implicit operator ActivityOptions(string resourcePath)
            => new(new ViewOptions(resourcePath));

        public static implicit operator ViewOptions(in ActivityOptions options)
            => options.options;
    }
}
