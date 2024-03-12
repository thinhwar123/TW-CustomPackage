using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TW.UGUI.AssetLoaders
{
    [CreateAssetMenu(fileName = "PreloadedAssetLoader", menuName = "Screen Navigator/Loaders/Preloaded Asset Loader")]
    public sealed class PreloadedAssetLoaderObject : AssetLoaderObject, IAssetLoader
    {
        [field: SerializeField] public List<KeyAssetPair> PreloadedObjects { get; private set; } = new();
        private PreloadedAssetLoader Loader { get; } = new();

        private void OnEnable()
        {
            if (!Application.isPlaying)
                return;

            foreach (var preloadedObject in PreloadedObjects)
            {
                if (string.IsNullOrEmpty(preloadedObject.Key))
                    continue;

                if (Loader.PreloadedObjects.ContainsKey(preloadedObject.Key))
                    continue;

                Loader.PreloadedObjects.Add(preloadedObject.Key, preloadedObject.Asset);
            }
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
                return;

            Loader.PreloadedObjects.Clear();
        }

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

        [Serializable]
        public sealed class KeyAssetPair
        {
            public enum KeySourceType
            {
                InputField,
                AssetName
            }

            [SerializeField] private KeySourceType _keySource = KeySourceType.AssetName;
            [ShowIf("@_keySource == KeySourceType.AssetName")]
            [SerializeField] private string _key;
            [ShowIf("@_keySource == KeySourceType.InputField")]
            [SerializeField] private Object _asset;

            public KeySourceType KeySource
            {
                get => _keySource;
                set => _keySource = value;
            }
            [ShowInInspector]
            public string Key
            {
                get => GetKey();
                set => _key = value;
            }

            public Object Asset
            {
                get => _asset;
                set => _asset = value;
            }

            private string GetKey()
            {
                if (_keySource == KeySourceType.AssetName)
                    return _asset == null ? "" : _asset.name;
                return _key;
            }
        }
    }
}