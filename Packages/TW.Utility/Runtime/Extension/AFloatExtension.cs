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
    }

}