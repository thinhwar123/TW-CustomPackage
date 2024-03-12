using Cysharp.Threading.Tasks;
using TW.UGUI.AssetLoaders;

namespace TW.UGUI.Core.Views
{
    public interface IViewContainer
    {

        /// <summary>
        /// By default, <see cref="IAssetLoader" /> in <see cref="UnityScreenNavigatorSettings" /> is used.
        /// If this property is set, it is used instead.
        /// </summary>
        IAssetLoader AssetLoader { get; set; }

        /// <summary>
        /// Returns the number of view instances currently in the pool
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        bool ContainsInPool(string resourcePath);

        /// <summary>
        /// Returns true if there is at least one view instance in the pool.
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        int CountInPool(string resourcePath);

        /// <summary>
        /// Only keep an amount of view instances in the pool,
        /// destroy other redundant instances.
        /// </summary>
        /// <param name="resourcePath">Resource path of the view</param>
        /// <param name="amount">The number of view instances to keep</param>
        /// <remarks>Fire-and-forget</remarks>
        void KeepInPool(string resourcePath, int amount);

        /// <summary>
        /// Only keep an amount of view instances in the pool,
        /// destroy other redundant instances.
        /// </summary>
        /// <param name="resourcePath">Resource path of the view</param>
        /// <param name="amount">The number of view instances to keep</param>
        /// <remarks>Asynchronous</remarks>
        UniTask KeepInPoolAsync(string resourcePath, int amount);

        /// <summary>
        /// Preload an amount of view instances and keep them in the pool.
        /// </summary>
        /// <remarks>Fire-and-forget</remarks>
        void Preload(string resourcePath, bool loadAsync = true, int amount = 1);

        /// <summary>
        /// Preload an amount of view instances and keep them in the pool.
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        UniTask PreloadAsync(string resourcePath, bool loadAsync = true, int amount = 1);
    }
}