﻿using System;
using System.Collections.Generic;
using TW.UGUI.Core.Views;
using TW.UGUI.Utility;
using UnityEngine;

namespace TW.UGUI.Core.Windows
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform), typeof(Canvas))]
    public class WindowContainerManager : View, IWindowContainerManager
    {
        private readonly List<IWindowContainer> _containers = new();

        public IReadOnlyList<IWindowContainer> Containers => _containers;

        public void Add(IWindowContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (_containers.Contains(container))
                return;

            _containers.Add(container);

            if (container.TryGetTransform(out var layerTransform))
                transform.AddChild(layerTransform);
        }

        public bool Remove(IWindowContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            return _containers.Remove(container);
        }

        public T Find<T>() where T : IWindowContainer
        {
            if (TryFind<T>(out var container))
                return container;

            Debug.LogError($"Cannot find layer of type {typeof(T).Name}");
            return default;
        }

        public T Find<T>(string containerName) where T : IWindowContainer
        {
            if (TryFind<T>(containerName, out var container))
                return container;

            Debug.LogError($"Cannot find layer {containerName}");
            return default;
        }

        public bool TryFind<T>(out T container) where T : IWindowContainer
        {
            container = default;

            var count = _containers.Count;

            for (var i = 0; i < count; i++)
            {
                if (_containers[i] is T containerT)
                {
                    container = containerT;
                    break;
                }
            }

            return container != null;
        }

        public bool TryFind<T>(string containerName, out T container) where T : IWindowContainer
        {
            container = default;

            var count = _containers.Count;

            for (var i = 0; i < count; i++)
            {
                if (_containers[i] is T containerT && string.Equals(containerT.LayerName, containerName))
                {
                    container = containerT;
                    break;
                }
            }

            return container != null;
        }

        public void Clear()
        {
            for (var i = _containers.Count - 1; i >= 0; i--)
            {
                var container = _containers[i];
                _containers.RemoveAt(i);

                if (container.TryGetTransform(out var transform))
                {
                    Destroy(transform.gameObject);
                }
            }

            _containers.Clear();
        }
    }
}