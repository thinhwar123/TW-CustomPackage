using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TW.UGUI.AssetLoaders
{
    public sealed class ResourcesAssetLoader : IAssetLoader
    {
        private Dictionary<AssetLoadHandleId, AssetLoadHandle> ControlIdToHandles { get; } = new();
        private uint NextControlId { get; set; }

        public AssetLoadHandle<T> Load<T>(string key) where T : Object
        {
            uint controlId = NextControlId++;
            AssetLoadHandle<T> handle = new AssetLoadHandle<T>(controlId);
            ControlIdToHandles.Add(controlId, handle);

            T result = Resources.Load<T>(key);
            handle.SetResult(result);

            AssetLoadStatus status = result != null ? AssetLoadStatus.Success : AssetLoadStatus.Failed;
            handle.SetStatus(status);

            if (result == null)
            {
                InvalidOperationException exception = new InvalidOperationException($"Requested asset（Key: {key}）was not found.");
                handle.SetOperationException(exception);
            }

            handle.SetPercentCompleteFunc(() => 1.0f);
            handle.SetTask(UniTask.FromResult(result));
            return handle;
        }

        public AssetLoadHandle<T> LoadAsync<T>(string key) where T : Object
        {
            var controlId = NextControlId++;
            var handle = new AssetLoadHandle<T>(controlId);
            ControlIdToHandles.Add(controlId, handle);

            var tcs = new UniTaskCompletionSource<T>();
            var req = Resources.LoadAsync<T>(key);

            req.completed += _ =>
            {
                var result = req.asset as T;
                handle.SetResult(result);

                var status = result != null ? AssetLoadStatus.Success : AssetLoadStatus.Failed;
                handle.SetStatus(status);

                if (result == null)
                {
                    var exception = new InvalidOperationException($"Requested asset（Key: {key}）was not found.");
                    handle.SetOperationException(exception);
                }

                tcs.TrySetResult(result);
            };

            handle.SetPercentCompleteFunc(() => req.progress);
            handle.SetTask(tcs.Task);
            return handle;
        }


        public void Release(AssetLoadHandleId handleId)
        {
            if (ControlIdToHandles.Remove(handleId, out AssetLoadHandle handle) == false)
            {
                return;
            }
            handle.SetTypelessResult(null);
            // Resources.UnloadUnusedAssets() is responsible for releasing
            // assets loaded by Resources.Load(), so nothing is done here.
            // Don't use Resources.UnloadAsset.
        }
    }
}