using System;
using TW.UGUI.Core.Views;
using TW.UGUI.Shared;

namespace TW.UGUI.Core.Sheets
{
    public delegate void OnSheetLoadedCallback(int sheetId, Sheet sheet, Memory<object> args);

    public readonly struct SheetOptions
    {
        public readonly bool loadAsync;
        public readonly string resourcePath;
        public readonly PoolingPolicy poolingPolicy;
        public readonly OnSheetLoadedCallback onLoaded;

        public SheetOptions(
              string resourcePath
            , OnSheetLoadedCallback onLoaded = null
            , bool loadAsync = true
            , PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings
        )
        {
            this.loadAsync = loadAsync;
            this.resourcePath = resourcePath;
            this.onLoaded = onLoaded;
            this.poolingPolicy = poolingPolicy;
        }

        public ViewOptions AsViewOptions()
            => new(resourcePath, false, null, loadAsync, poolingPolicy);

        public static implicit operator SheetOptions(string resourcePath)
            => new(resourcePath);
    }
}