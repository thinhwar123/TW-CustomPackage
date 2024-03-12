using System;
using TW.UGUI.Core.Activities;
using TW.UGUI.Core.Modals;
using TW.UGUI.Core.Screens;
using TW.UGUI.Core.Windows;
using TW.UGUI.Shared;
using UnityEngine;


namespace ZBase.UnityScreenNavigator.Core
{
    [RequireComponent(typeof(RectTransform), typeof(Canvas))]
    public class UnityScreenNavigatorLauncher : WindowContainerManager
    {
        [field: SerializeField] private WindowContainerSettings WindowContainerSettings{ get; set; }

        protected override void Awake()
        {
            if (WindowContainerSettings == false)
            {
                throw new NullReferenceException(nameof(WindowContainerSettings));
            }
        }

        protected override void Start()
        {
            var layers = WindowContainerSettings.Containers.Span;

            foreach (var layer in layers)
            {
                switch (layer.containerType)
                {
                    case WindowContainerType.Modal:
                        ModalContainer.Create(layer, this, UnityScreenNavigatorSettings.Instance);
                        break;

                    case WindowContainerType.Screen:
                        ScreenContainer.Create(layer, this, UnityScreenNavigatorSettings.Instance);
                        break;

                    case WindowContainerType.Activity:
                        ActivityContainer.Create(layer, this, UnityScreenNavigatorSettings.Instance);
                        break;
                }
            }
        }
    }
}