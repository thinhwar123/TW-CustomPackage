using System;
using Sirenix.OdinInspector;
using TW.UGUI.Shared;
using UnityEngine.Serialization;

namespace TW.UGUI.Core.Windows
{
    [Serializable]
    public class WindowContainerConfig
    {
        public string name;

        [FormerlySerializedAs("layerType")]
        public WindowContainerType containerType;

        public bool overrideSorting;

        [ShowIf("@overrideSorting")]
        public SortingLayerId sortingLayer;

        [ShowIf("@overrideSorting")]
        public int orderInLayer = 0;
    }
}