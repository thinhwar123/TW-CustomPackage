using System.Linq;
using UnityEngine;

namespace TW.Utility.Extension
{
    public static class AColorExtension
    {
        public const string FormatGetColorInPalette = "@TW.Utility.Extension.AColorExtension.GetColorInPalette(\"{paletteName}\", (int)$value)";
        public const string FormatGetColorById = "@TW.Utility.Extension.AColorExtension.GetColorById((int)$value, {range})";
        public const string FormatGetColorInGlobalConfig = "@TW.Utility.Extension.AColorExtension.GetColorInGlobalConfig(\"{name}\", (int)$value)";

        /// <summary>
        /// Retrieves a color from the specified color palette with the specified index.
        /// Returns white if the color palette or index cannot be found.
        /// This method requires the Sirenix.OdinInspector.Editor namespace.
        /// </summary>
        /// <param name="colorPaletteName">The name of the color palette to search for the color in.</param>
        /// <param name="index">The index of the color in the color palette to retrieve.</param>
        /// <returns></returns>
        public static Color GetColorInPalette(string colorPaletteName, int index)
        {
#if UNITY_EDITOR
            Sirenix.OdinInspector.Editor.ColorPalette colorPalette = Sirenix.OdinInspector.Editor.ColorPaletteManager.Instance.ColorPalettes.FirstOrDefault(x => x.Name == colorPaletteName);
            return colorPalette != null ? colorPalette.Colors[index] : Color.white;
#else
            return Color.white;
#endif
        }
        /// <summary>
        /// Returns a color generated based on the provided ID.
        /// Colors with IDs closer to each other will have a greater difference. White text is easily visible on the background color.
        /// </summary>
        /// <param name="id">The ID used to generate the color.</param>
        /// <param name="range">The number of the color.</param>
        /// <returns>A color generated based on the provided ID.</returns>
        public static Color GetColorById(int id, int range)
        {
            // Define the range of hues for the colors (0-360 degrees)
            float hueRange = 360f;

            // Calculate the hue for this ID (wrapping around after 100 IDs)
            float hue = Mathf.Repeat((float)id / range * hueRange, hueRange);

            // Define the saturation and value for the colors
            float saturation = 0.7f;
            float value = 0.9f;

            // Adjust the hue based on neighboring IDs to increase the difference
            float hueAdjustment = Mathf.Clamp01(1f / 100f * hueRange);
            hue += hueAdjustment * ((id / 10) % 2 == 0 ? 1 : -1);

            // Create the color from the hue, saturation, and value
            Color color = Color.HSVToRGB(hue / hueRange, saturation, value);

            return color;
        }

        public static Color GetColorInGlobalConfig(string colorPaletteName, int index)
        {
            return EditorColorGlobalConfig.Instance.GetColor(colorPaletteName, index);
        }

        /// <summary>
        /// Returns string representation of the color in hexadecimal format.
        /// </summary>
        /// <param name="color">Origin color.</param>
        /// <returns>String hexadecimal format.</returns>
        public static string ToHex(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }
    } 
}