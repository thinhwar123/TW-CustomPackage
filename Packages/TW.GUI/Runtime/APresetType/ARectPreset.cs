using System.Collections.Generic;
using UnityEngine;

namespace TW.GUI
{
    public class ARectPreset : APreset
    {
#if UNITY_EDITOR
        [field: SerializeField] public RectTransform RectTransform {get; private set;}
        
        public override void OnReset()
        {
            base.OnReset();
            RectTransform = GetComponent<RectTransform>();
            PresetProperties.PresetProperties = new List<APresetProperty>()
            { 
                new APresetProperty(APresetProperty.Type.RectPosition),
                new APresetProperty(APresetProperty.Type.RectSize), 
                new APresetProperty(APresetProperty.Type.RectPivot), 
                new APresetProperty(APresetProperty.Type.RectAnchor), 
                new APresetProperty(APresetProperty.Type.RectPivot), 
                new APresetProperty(APresetProperty.Type.RectRotation), 
                new APresetProperty(APresetProperty.Type.RectScale),
            };
        }

        public override void OnInspectorInit()
        {
            base.OnInspectorInit();
            if (RectTransform == null)
            {
                RectTransform = GetComponent<RectTransform>();
            }
            PresetProperties.Init(AVisualElement.EType.Rect);
        }
#endif
    }
}
