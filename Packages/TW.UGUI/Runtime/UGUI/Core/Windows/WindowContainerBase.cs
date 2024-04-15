using System;
using TW.UGUI.Core.Views;
using TW.UGUI.Shared;
using TW.UGUI.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace TW.UGUI.Core.Windows
{
    public abstract class WindowContainerBase : ViewContainerBase, IWindowContainer
    {
        public string LayerName { get; private set; }

        public WindowContainerType LayerType { get; private set; }

        public IWindowContainerManager ContainerManager { get; private set; }

        public Canvas Canvas { get; private set; }

        protected WindowContainerConfig Config { get; private set; }

        public void Initialize(
              WindowContainerConfig config
            , IWindowContainerManager manager
            , UnityScreenNavigatorSettings settings
        )
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Settings = settings ? settings : throw new ArgumentNullException(nameof(settings));

            ContainerManager = manager ?? throw new ArgumentNullException(nameof(manager));
            ContainerManager.Add(this);

            LayerName = config.name;
            LayerType = config.containerType;
            
            var canvas = GetComponent<Canvas>();

            if (config.overrideSorting)
            {
                canvas.overrideSorting = true;
                canvas.sortingLayerID = config.sortingLayer.id;
                canvas.sortingOrder = config.orderInLayer;
            }

            Canvas = canvas;

            InitializePool();
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        protected override void InitializePool()
        {
            base.InitializePool();

            var poolCanvas = PoolTransform.gameObject.GetOrAddComponent<Canvas>();
            poolCanvas.overrideSorting = true;
            poolCanvas.sortingLayerID = Canvas.sortingLayerID;
            poolCanvas.sortingOrder = Canvas.sortingOrder;
        }
    }
}