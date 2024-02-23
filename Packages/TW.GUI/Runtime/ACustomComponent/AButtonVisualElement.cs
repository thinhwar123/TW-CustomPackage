using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TW.GUI
{
    public class AButtonVisualElement : AVisualElement
    {
        [field: SerializeField] public ACustomButton MainButton { get; private set; }
    }
}
