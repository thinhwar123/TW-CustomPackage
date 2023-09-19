using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class AUIText : AUIVisualElement
{
    [field: SerializeField, InlineEditor] public AUITextConfig AUITextConfig {get; private set;}
    [field: SerializeField] public TextMeshProUGUI TextField {get; private set;}
    [field: SerializeField] public Color TextColor {get; private set;}
    
    [field: ValueDropdown(nameof(CustomChangeTextPreset), DrawDropdownForListElements = false)]
    [field: SerializeField] public string TextPreset {get; private set;}
    public string Text
    {
        get => TextField.text;
        set => TextField.text = value;
    }

    protected override void Setup()
    {
        if (TextField == null)
        {
            TextField = GetComponentInChildren<TextMeshProUGUI>();
            TextColor = TextField.color;   
        }
    }

    protected override void Config()
    {
        TextField.color = TextColor;
        if(AUITextConfig == null) return;
        TextField.font = AUITextConfig.TextFontAsset;
        TextField.spriteAsset = AUITextConfig.TextSpriteAsset;
        
        AUITextConfig.Preset preset = AUITextConfig.Presets.FirstOrDefault(p => p.m_PresetName == TextPreset);
        if (preset.m_PresetName.IsNullOrWhitespace()) return;
        
        TextField.fontSize = preset.m_PresetSize;
        
        TextField.fontStyle = (FontStyles)preset.m_PresetStyle;
        
    }
    private IEnumerable CustomChangeTextPreset()
    {
        if (AUITextConfig == null) return null;
        return AUITextConfig.Presets.Select(x => x.m_PresetName);
    }
}
