using UnityEngine;

namespace TW.Utility.Extension
{
    public static class AFloatExtension
    {
        /// <summary>
        /// This method extends the float class and rounds a given float value to a specified number of decimal places. The rounded float value is returned.
        /// </summary>
        /// <param name="value">The float value to round to a certain number of decimal places.</param>
        /// <param name="decimalPlaces">The number of decimal places to round the float value to.</param>
        /// <returns></returns>
        public static float RoundFloat(this float value, int decimalPlaces)
        {
            return Mathf.Round(value * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
        }
        /// <summary>
        /// Remaps a value from one range to another.
        /// </summary>
        /// <param name="value">The original value to be remapped.</param>
        /// <param name="fromMin">The minimum value of the original range.</param>
        /// <param name="fromMax">The maximum value of the original range.</param>
        /// <param name="toMin">The minimum value of the target range.</param>
        /// <param name="toMax">The maximum value of the target range.</param>
        /// <returns>The remapped value in the target range.</returns>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float normalizedValue = Mathf.InverseLerp(fromMin, fromMax, value);
            return Mathf.Lerp(toMin, toMax, normalizedValue);
        }
    }

}