using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using TW.UGUI.AssetLoaders;
using TW.UGUI.Shared;
using TW.UGUI.Utility;
using UnityEngine;
using UnityEngine.UI;


namespace TW.UGUI.Core.Views
{
    public abstract class ViewContainerBase : View, IViewContainer
    {
        /// <summary>
        /// One asset handle per resource path, shared by every view instantiated from it.
        /// A handle is in this map exactly while <see cref="_resourcePathToRefCount"/> has a count for its path.
        /// </summary>
        private readonly Dictionary<string, AssetLoadHandle<GameObject>> _resourcePathToHandle = new();

        /// <summary>
        /// Live views (in use or pooled) plus loads in progress for each resource path.
        /// The handle is released only when this reaches 0, so destroying one view never unloads
        /// the asset (and possibly its whole bundle) out from under another view of the same path.
        /// </summary>
        private readonly Dictionary<string, int> _resourcePathToRefCount = new();

        /// <summary>
        /// Views that hold one reference each. Destroying a view releases its reference once, even if destroyed twice.
        /// </summary>
        private readonly HashSet<View> _referencedViews = new();

        private readonly Dictionary<string, Queue<View>> _resourcePathToPool = new();

        private IAssetLoader _assetLoader;

        /// <summary>
        /// By default, <see cref="IAssetLoader" /> in <see cref="UnityScreenNavigatorSettings" /> is used.
        /// If this property is set, it is used instead.
        /// </summary>
        public IAssetLoader AssetLoader
        {
            get => _assetLoader ?? Settings.AssetLoader;
            set => _assetLoader = value ?? throw new ArgumentNullException(nameof(value));
        }

        protected bool EnablePooling
        {
            get => Settings.EnablePooling;
        }

        protected RectTransform PoolTransform { get; set; }

        protected virtual void InitializePool()
        {
            var parentTransform = this.transform.parent.GetComponent<RectTransform>();

            var poolGO = new GameObject(
                  $"[Pool] {this.name}"
                , typeof(CanvasGroup)
                , typeof(LayoutElement)
            );

            PoolTransform = poolGO.GetOrAddComponent<RectTransform>();
            PoolTransform.SetParent(parentTransform, false);
            PoolTransform.FillParent(parentTransform);

            var poolCanvasGroup = poolGO.GetComponent<CanvasGroup>();
            poolCanvasGroup.alpha = 0f;
            poolCanvasGroup.blocksRaycasts = false;
            poolCanvasGroup.interactable = false;

            var layoutElement = poolGO.GetComponent<LayoutElement>();
            layoutElement.ignoreLayout = true;
        }

        protected override void OnDestroy()
        {
            foreach (var (resourcePath, pool) in _resourcePathToPool)
            {
                while (pool.TryDequeue(out var view))
                {
                    DestroyAndForget(view, resourcePath, PoolingPolicy.DisablePooling).Forget();
                }
            }

            _resourcePathToPool.Clear();
        }

        /// <summary>
        /// Returns the number of view instances currently in the pool
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public int CountInPool(string resourcePath)
            => _resourcePathToPool.TryGetValue(resourcePath, out var pool) ? pool.Count : 0;

        /// <summary>
        /// Returns true if there is at least one view instance in the pool.
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public bool ContainsInPool(string resourcePath)
            => _resourcePathToPool.TryGetValue(resourcePath, out var pool) && pool.Count > 0;

        /// <summary>
        /// Only keep an amount of view instances in the pool,
        /// destroy other redundant instances.
        /// </summary>
        /// <param name="resourcePath">Resource path of the view</param>
        /// <param name="amount">The number of view instances to keep</param>
        /// <remarks>Fire-and-forget</remarks>
        public void KeepInPool(string resourcePath, int amount)
        {
            KeepInPoolAndForget(resourcePath, amount).Forget();
        }

        private async UniTaskVoid KeepInPoolAndForget(string resourcePath, int amount)
        {
            await KeepInPoolAsync(resourcePath, amount);
        }

        /// <summary>
        /// Only keep an amount of view instances in the pool,
        /// destroy other redundant instances.
        /// </summary>
        /// <param name="resourcePath">Resource path of the view</param>
        /// <param name="amount">The number of view instances to keep</param>
        /// <remarks>Asynchronous</remarks>
        public async UniTask KeepInPoolAsync(string resourcePath, int amount)
        {
            if (_resourcePathToPool.TryGetValue(resourcePath, out var pool) == false)
            {
                return;
            }

            var amountToDestroy = pool.Count - Mathf.Clamp(amount, 0, pool.Count);

            if (amountToDestroy < 1)
            {
                return;
            }

            var doDestroying = false;
            var referencesToRemove = 0;

            for (var i = 0; i < amountToDestroy; i++)
            {
                if (pool.TryDequeue(out var view))
                {
                    if (StopReferencing(view))
                    {
                        referencesToRemove++;
                    }

                    if (view && view.gameObject)
                    {
                        Destroy(view.gameObject);
                        doDestroying = true;
                    }
                }
            }

            if (doDestroying)
            {
                await UniTask.NextFrame();
            }

            // Views still in use keep the handle alive; it is released with the last reference.
            for (var i = 0; i < referencesToRemove; i++)
            {
                RemoveReference(resourcePath);
            }
        }

        /// <summary>
        /// Preload an amount of view instances and keep them in the pool.
        /// </summary>
        /// <remarks>Fire-and-forget</remarks>
        public void Preload(string resourcePath, bool loadAsync = true, int amount = 1)
        {
            PreloadAndForget(resourcePath, loadAsync, amount).Forget();
        }

        private async UniTaskVoid PreloadAndForget(string resourcePath, bool loadAsync = true, int amount = 1)
        {
            await PreloadAsync(resourcePath, loadAsync, amount);
        }

        /// <summary>
        /// Preload an amount of view instances and keep them in the pool.
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        public async UniTask PreloadAsync(string resourcePath, bool loadAsync = true, int amount = 1)
        {
            if (amount < 1)
            {
                Debug.LogWarning($"The amount of preloaded view instances should be greater than 0.");
                return;
            }

            if (_resourcePathToPool.TryGetValue(resourcePath, out var pool) == false)
            {
                _resourcePathToPool[resourcePath] = pool = new Queue<View>();
            }

            if (pool.Count >= amount)
            {
                return;
            }

            // Held while loading; each pooled instance then holds its own reference.
            AddReference(resourcePath);

            try
            {
                var assetLoadHandle = await GetLoadedHandleAsync(resourcePath, loadAsync);
                var differentAmount = amount - pool.Count;

                for (var i = 0; i < differentAmount; i++)
                {
                    InstantiateToPool(resourcePath, assetLoadHandle, pool);
                }
            }
            finally
            {
                RemoveReference(resourcePath);
            }
        }

        private void InstantiateToPool(string resourcePath, AssetLoadHandle<GameObject> assetLoadHandle, Queue<View> pool)
        {
            var instance = Instantiate(assetLoadHandle.Result);

            if (instance.TryGetComponent<View>(out var view) == false)
            {
                Debug.LogError(
                    $"Cannot find the {typeof(View).Name} component on the specified resource `{resourcePath}`."
                    , instance
                );

                Destroy(instance);
                return;
            }

            AddReference(resourcePath);
            _referencedViews.Add(view);

            view.Settings = Settings;
            view.RectTransform.SetParent(PoolTransform);
            view.Parent = PoolTransform;
            view.Owner.SetActive(false);

            pool.Enqueue(view);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected async UniTask<T> GetViewAsync<T>(ViewOptions options)
            where T : View
            => await GetViewAsync<T>(options.resourcePath, options.loadAsync, options.poolingPolicy);

        protected async UniTask<T> GetViewAsync<T>(string resourcePath, bool loadAsync, PoolingPolicy poolingPolicy)
            where T : View
        {
            if (GetFromPool<T>(resourcePath, poolingPolicy, out var existView))
            {
                existView.Settings = Settings;
                return existView;
            }

            // Taken before awaiting: a load in progress keeps the handle alive
            // even if the last live view of this path is destroyed meanwhile.
            AddReference(resourcePath);

            T view = null;

            try
            {
                var assetLoadHandle = await GetLoadedHandleAsync(resourcePath, loadAsync);
                var instance = Instantiate(assetLoadHandle.Result);

                if (instance.TryGetComponent(out view) == false)
                {
                    Debug.LogError(
                        $"Cannot find the {typeof(T).Name} component on the specified resource `{resourcePath}`."
                        , instance
                    );

                    Destroy(instance);
                    return null;
                }

                view.Settings = Settings;
                _referencedViews.Add(view);
                return view;
            }
            finally
            {
                // No view came out of this call (load failed, wrong prefab): give the reference back.
                if (view == false)
                {
                    RemoveReference(resourcePath);
                }
            }
        }

        /// <summary>
        /// Returns the loaded handle of <paramref name="resourcePath"/>, starting the load if none is in progress.
        /// The caller must hold a reference (<see cref="AddReference"/>) for the whole call.
        /// </summary>
        private async UniTask<AssetLoadHandle<GameObject>> GetLoadedHandleAsync(string resourcePath, bool loadAsync)
        {
            if (_resourcePathToHandle.TryGetValue(resourcePath, out var assetLoadHandle) == false)
            {
                assetLoadHandle = loadAsync
                    ? AssetLoader.LoadAsync<GameObject>(resourcePath)
                    : AssetLoader.Load<GameObject>(resourcePath);

                // Registered before the load completes so concurrent requests for the same path share it.
                _resourcePathToHandle[resourcePath] = assetLoadHandle;
            }

            while (assetLoadHandle.IsDone == false)
            {
                await UniTask.NextFrame();
            }

            if (assetLoadHandle.Status == AssetLoadStatus.Failed)
            {
                // The failed handle stays registered until its last reference is removed, then it is released.
                throw assetLoadHandle.OperationException
                      ?? new InvalidOperationException($"Failed to load the resource `{resourcePath}`.");
            }

            return assetLoadHandle;
        }

        private void AddReference(string resourcePath)
        {
            _resourcePathToRefCount.TryGetValue(resourcePath, out var count);
            _resourcePathToRefCount[resourcePath] = count + 1;
        }

        private void RemoveReference(string resourcePath)
        {
            if (_resourcePathToRefCount.TryGetValue(resourcePath, out var count) == false)
            {
                return;
            }

            if (count > 1)
            {
                _resourcePathToRefCount[resourcePath] = count - 1;
                return;
            }

            _resourcePathToRefCount.Remove(resourcePath);

            if (_resourcePathToHandle.Remove(resourcePath, out var handle))
            {
                AssetLoader.Release(handle.Id);
            }
        }

        /// <summary>
        /// True if <paramref name="view"/> held a reference, which the caller must now remove.
        /// Compared by reference, so a view that is already destroyed still gives its reference back.
        /// </summary>
        private bool StopReferencing(View view)
        {
            return ReferenceEquals(view, null) == false && _referencedViews.Remove(view);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void DestroyAndForget<T>(ViewRef<T> viewRef) where T : View
            => DestroyAndForget(viewRef.View, viewRef.ResourcePath, viewRef.PoolingPolicy).Forget();

        protected async UniTaskVoid DestroyAndForget<T>(T view, string resourcePath, PoolingPolicy poolingPolicy)
            where T : View
        {
            if (ReturnToPool(view, resourcePath, poolingPolicy))
            {
                return;
            }

            // Taken out before the await so destroying the same view twice never releases twice.
            var hadReference = StopReferencing(view);

            if (view && view.Owner)
            {
                Destroy(view.Owner);
                await UniTask.NextFrame();
            }

            // Other views of this path (in use or pooled) keep the handle; the last one releases it.
            if (hadReference)
            {
                RemoveReference(resourcePath);
            }
        }

        protected bool GetFromPool<T>(string resourcePath, PoolingPolicy poolingPolicy, out T view)
            where T : View
        {
            if (CanPool(poolingPolicy)
                && _resourcePathToPool.TryGetValue(resourcePath, out var pool)
                && pool.TryDequeue(out var typelessView)
            )
            {
                if (typelessView is T typedView)
                {
                    view = typedView;
                    view.Settings = Settings;
                    view.Owner.SetActive(true);
                    return true;
                }

                DestroyAndForget(typelessView, resourcePath, PoolingPolicy.DisablePooling).Forget();
            }

            view = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool ReturnToPool<T>(ViewRef<T> viewRef)
            where T : View
            => ReturnToPool<T>(viewRef.View, viewRef.ResourcePath, viewRef.PoolingPolicy);

        protected bool ReturnToPool<T>(T view, string resourcePath, PoolingPolicy poolingPolicy)
            where T : View
        {
            if (view == false)
            {
                return false;
            }

            if (CanPool(poolingPolicy) == false)
            {
                return false;
            }

            if (_resourcePathToPool.TryGetValue(resourcePath, out var pool) == false)
            {
                _resourcePathToPool[resourcePath] = pool = new Queue<View>();
            }

            if (view.Owner == false)
            {
                return false;
            }

            view.RectTransform.SetParent(PoolTransform);
            view.Parent = PoolTransform;
            view.Owner.SetActive(false);
            pool.Enqueue(view);
            return true;
        }

        protected bool CanPool(PoolingPolicy poolingPolicy)
        {
            if (poolingPolicy == PoolingPolicy.DisablePooling)
                return false;

            if (poolingPolicy == PoolingPolicy.EnablePooling)
                return true;

            return EnablePooling;
        }
    }
}
