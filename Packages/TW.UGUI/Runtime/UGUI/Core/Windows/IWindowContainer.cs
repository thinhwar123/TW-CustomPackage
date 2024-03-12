using TW.UGUI.Core.Views;
using UnityEngine;

namespace TW.UGUI.Core.Windows
{
    public interface IWindowContainer : IViewContainer
    {
        string LayerName { get; }

        WindowContainerType LayerType { get; }

        IWindowContainerManager ContainerManager { get; }

        Canvas Canvas { get; }
    }
}