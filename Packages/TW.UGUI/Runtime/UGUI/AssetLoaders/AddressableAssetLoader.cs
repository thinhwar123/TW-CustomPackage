using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace TW.UGUI.AssetLoaders
{
public sealed class AddressableAssetLoader : IAssetLoader
    {
        private Dictionary<AssetLoadHandleId, AsyncOperationHandle> ControlIdToHandles { get; set; } = new();
        private uint NextControlId { get; set; }

        public bool SuppressErrorLogOnRelease { get; set; }

        public AssetLoadHandle<T> Load<T>(string key)
            where T : Object
        {
            AsyncOperationHandle<T> addressableHandle = Addressables.LoadAssetAsync<T>(key);
            T result = addressableHandle.WaitForCompletion();

            uint controlId = NextControlId++;
            ControlIdToHandles.Add(controlId, addressableHandle);

            AssetLoadHandle<T> handle = new AssetLoadHandle<T>(controlId);
            handle.SetPercentCompleteFunc(() => addressableHandle.PercentComplete);
            handle.SetTask(UniTask.FromResult(result));
            handle.SetResult(result);

            AssetLoadStatus status = addressableHandle.Status == AsyncOperationStatus.Succeeded
                ? AssetLoadStatus.Success
                : AssetLoadStatus.Failed;

            handle.SetStatus(status);
            handle.SetOperationException(addressableHandle.OperationException);

            return handle;

        }

        public AssetLoadHandle<T> LoadAsync<T>(string key)
            where T : Object
        {
            AsyncOperationHandle<T> addressableHandle = Addressables.LoadAssetAsync<T>(key);

            uint controlId = NextControlId++;
            ControlIdToHandles.Add(controlId, addressableHandle);

            AssetLoadHandle<T> handle = new AssetLoadHandle<T>(controlId);
            UniTaskCompletionSource<T> tcs = new UniTaskCompletionSource<T>();
            addressableHandle.Completed += x =>
            {
                handle.SetResult(x.Result);

                var status = x.Status == AsyncOperationStatus.Succeeded
                    ? AssetLoadStatus.Success
                    : AssetLoadStatus.Failed;

                handle.SetStatus(status);
                handle.SetOperationException(addressableHandle.OperationException);
                tcs.TrySetResult(x.Result);
            };

            handle.SetPercentCompleteFunc(() => addressableHandle.PercentComplete);
            handle.SetTask(tcs.Task);

            return handle;
        }

        public void Release(AssetLoadHandleId handleId)
        {
            if (ControlIdToHandles.Remove(handleId, out AsyncOperationHandle handle) == false)
            {
                if (SuppressErrorLogOnRelease == false)
                {
                    UnityEngine.Debug.LogError(
                        $"There is no asset that has been requested for release (Handle.Id: {handleId})."
                    );
                }

                return;
            }

            Addressables.Release(handle);
        }
    }
}