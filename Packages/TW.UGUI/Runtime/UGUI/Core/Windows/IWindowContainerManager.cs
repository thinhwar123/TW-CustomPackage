using System.Collections.Generic;

namespace TW.UGUI.Core.Windows
{
    public interface IWindowContainerManager
    {
        IReadOnlyList<IWindowContainer> Containers { get; }

        void Add(IWindowContainer container);

        bool Remove(IWindowContainer container);

        T Find<T>() where T : IWindowContainer;

        T Find<T>(string containerName) where T : IWindowContainer;

        bool TryFind<T>(out T container) where T : IWindowContainer;

        bool TryFind<T>(string containerName, out T container) where T : IWindowContainer;

        void Clear();
    }
}