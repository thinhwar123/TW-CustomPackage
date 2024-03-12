using UnityEngine;

namespace TW.UGUI.AssetLoaders
{
    [CreateAssetMenu(fileName = "AddressableAssetLoader", menuName = "Screen Navigator/Loaders/Addressable Asset Loader")]
    public sealed class AddressableAssetLoaderObject : AssetLoaderObject, IAssetLoader
    {
        [field: SerializeField] private bool SuppressErrorLogOnRelease { get; set; }

        private AddressableAssetLoader Loader { get; } = new();

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
            Loader.SuppressErrorLogOnRelease = SuppressErrorLogOnRelease;
            Loader.Release(handleId);
        }
    }
}