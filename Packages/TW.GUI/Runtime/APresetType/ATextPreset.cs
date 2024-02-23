using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TW.GUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ATextPreset : APreset
    {
#if UNITY_EDITOR
        [field: SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; set; }
        public override void OnReset()
        {
            base.OnReset();
            TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            PresetProperties.PresetProperties = new List<APresetProperty>()
            { 
                new APresetProperty(APresetProperty.Type.TextFont), 
                new APresetProperty(APresetProperty.Type.TextFontStyle), 
                new APresetProperty(APresetProperty.Type.TextFontSize), 
                new APresetProperty(APresetProperty.Type.TextAutoSize),
                new APresetProperty(APresetProperty.Type.TextColor),
                new APresetProperty(APresetProperty.Type.TextSpacingOption),
                new APresetProperty(APresetProperty.Type.TextAlignment),
                new APresetProperty(APresetProperty.Type.TextSpriteAsset),
                new APresetProperty(APresetProperty.Type.TextStyleSheetsAsset)
            };
        }

        public override void OnInspectorInit()
        {
            base.OnInspectorInit();
            if (TextMeshProUGUI == null)
            {
                TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            }
            PresetProperties.Init(AVisualElement.EType.Text);
        }
#endif
    }
}