using UnityEngine;

namespace TW.UGUI.AssetLoaders
{
    [CreateAssetMenu(fileName = "ResourcesAssetLoader", menuName = "Screen Navigator/Loaders/Resource Asset Loader")]
    public sealed class ResourcesAssetLoaderObject : AssetLoaderObject, IAssetLoader
    {
        private ResourcesAssetLoader Loader { get; } = new();

        public override AssetLoadHandle<T> Load<T>(string key)
        {
            return Loader.Load<T>(key);
        }

        public override AssetLoadHandle<T> LoadAsync<T>(string key)
        {
            return Loader.LoadAsync<T>(key);
        }

        public override void Release(AssetLoadHandleId handleId)
        {
            Loader.Release(handleId);
        }
    }
}