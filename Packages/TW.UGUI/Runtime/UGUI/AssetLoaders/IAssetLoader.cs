using UnityEngine;

namespace TW.UGUI.AssetLoaders
{
    public interface IAssetLoader
    {
        AssetLoadHandle<T> Load<T>(string key) where T : Object;

        AssetLoadHandle<T> LoadAsync<T>(string key) where T : Object;

        void Release(AssetLoadHandleId handle);
    }
}