using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TW.GUI
{
    public class AImagePreset : APreset
    {
#if UNITY_EDITOR
        [field: SerializeField] public Image Image { get; private set; }
        
        public override void OnReset()
        {
            base.OnReset();
            Image = GetComponent<Image>();
            PresetProperties.PresetProperties = new List<APresetProperty>()
            { 
                new APresetProperty(APresetProperty.Type.ImageSprite),
                new APresetProperty(APresetProperty.Type.ImageColor),
            };
        }

        public override void OnInspectorInit()
        {
            base.OnInspectorInit();
            if (Image == null)
            {
                Image = GetComponent<Image>();
            }
            PresetProperties.Init(AVisualElement.EType.Image);
        }
#endif
    }
}
