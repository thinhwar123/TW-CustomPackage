using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "EditorColorGlobalConfig", menuName = "GlobalConfigs/EditorColorGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class EditorColorGlobalConfig : GlobalConfig<EditorColorGlobalConfig>
{
    [field: SerializeField] public ColorPalette[] ColorPalettes { get; private set;}

    public Color GetColor(string colorPaletteName, int index)
    {
        return ColorPalettes.FirstOrDefault(x => x.ColorPaletteName == colorPaletteName)?.GetColor(index) ?? Color.white;
    }
}
[System.Serializable]
public class ColorPalette
{
    [field: SerializeField] public string ColorPaletteName { get; private set;}
    [ListDrawerSettings(CustomAddFunction = nameof(CustomAddColor))]
    [field: SerializeField] public Color[] Colors {get; private set;}

    public Color GetColor(int index)
    {
        return Colors[(int)Mathf.Repeat(index, Colors.Length)];
    }

    public Color CustomAddColor()
    {
        return Color.white;
    }
}