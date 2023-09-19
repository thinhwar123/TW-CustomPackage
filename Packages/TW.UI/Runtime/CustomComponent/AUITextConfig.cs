using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "AUITextConfig", menuName = "AUIConfig/AUITextConfig")]
public class AUITextConfig : ScriptableObject
{
    [System.Serializable]
    public struct Preset
    {
        public string m_PresetName;
        public float m_PresetSize;
        [EnumToggleButtons]
        public MyFontStyles m_PresetStyle;
    }
    [Flags]
    public enum MyFontStyles
    {
        [HideInInspector] Normal = 0,
        [LabelText(" ", SdfIconType.TypeBold), LabelWidth(10)] Bold = 1,
        [LabelText(" ", SdfIconType.TypeItalic), LabelWidth(10)] Italic = 2,
        [LabelText(" ", SdfIconType.TypeUnderline), LabelWidth(10)] Underline = 4,
        [LabelText("ab"), LabelWidth(10)] LowerCase = 8,
        [LabelText("AB"), LabelWidth(10)] UpperCase = 16, // 0x00000010
        [LabelText("SC"), LabelWidth(10)] SmallCaps = 32, // 0x00000020
        [LabelText(" ", SdfIconType.TypeStrikethrough), LabelWidth(10)] Strikethrough = 64, // 0x00000040
        [HideInInspector] Superscript = 128, // 0x00000080
        [HideInInspector] Subscript = 256, // 0x00000100
        [HideInInspector] Highlight = 512, // 0x00000200
    }
    [field: SerializeField] public TMP_FontAsset TextFontAsset {get; private set;}
    [field: SerializeField] public TMP_SpriteAsset TextSpriteAsset {get; private set;}
    [field: SerializeField] public TMP_StyleSheet TextStyleSheet {get; private set;}
    
    [field: SerializeField] public Preset[] Presets {get; private set;}
    
}

