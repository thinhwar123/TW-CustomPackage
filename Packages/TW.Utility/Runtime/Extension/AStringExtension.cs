using System;
using System.Globalization;

namespace TW.Utility.Extension
{
    public static class AStringExtension
    {
        /// <summary>
        /// Converts a value of type T to a hexadecimal string representation with a given character count.
        /// Supported types include float, int, and enums.
        /// </summary>
        /// <typeparam name="T">The type of the value to convert.</typeparam>
        /// <param name="value">The value to convert to a hexadecimal string.</param>
        /// <param name="charCount">The number of characters in the resulting hexadecimal string.</param>
        /// <returns>The hexadecimal string representation of the input value.</returns>
        public static string ValueToHexString<T>(this T value, int charCount = 8)
        {
            Type type = typeof(T);
            string hexString = string.Empty;

            if (type == typeof(float))
            {
                hexString = (Convert.ToInt32(value) * 100).ToString($"X{charCount}");
            }
            else if (type == typeof(int))
            {
                hexString = Convert.ToInt32(value).ToString($"X{charCount}");
            }
            else if (type.IsEnum)
            {
                hexString = Convert.ToInt32(value).ToString($"X{charCount}");
            }

            return hexString;
        }
        /// <summary>
        /// Converts a hexadecimal string representation to a value of type T.
        /// Supported types include float, int, and enums.
        /// </summary>
        /// <typeparam name="T">The type of the value to convert to.</typeparam>
        /// <param name="hexString">The hexadecimal string to convert to a value of type T.</param>
        /// <returns>The value of type T represented by the input hexadecimal string.</returns>
        public static T HexStringToValue<T>(this string hexString)
        {
            Type type = typeof(T);

            if (type == typeof(float))
            {
                int hexValue = int.Parse(hexString, NumberStyles.HexNumber);
                return (T)(object)(hexValue / 100f);
            }
            else if (type == typeof(int))
            {
                int hexValue = int.Parse(hexString, NumberStyles.HexNumber);
                return (T)(object)hexValue;
            }
            else if (type.IsEnum)
            {
                int hexValue = int.Parse(hexString, NumberStyles.HexNumber);
                return (T)(object)hexValue;
            }

            throw new ArgumentException($"Type {type.Name} is not supported.");
        }
        /// <summary>
        /// Converts an enumeration value of type T to its integer representation as a string.
        /// </summary>
        /// <param name="enumType">The enumeration value to be converted.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>A string representation of the integer value corresponding to the enumeration value.</returns>
        public static string ToIntString<T>(this T enumType) where T : System.Enum
        {
            return $"{((int)(object)enumType).ToString()}";
        }
        /// <summary>
        /// Converts the input string to CamelCase format by making the first letter lowercase and preserving the rest of the string.
        /// </summary>
        /// <param name="str">The input string to be converted to CamelCase.</param>
        /// <returns>A string in CamelCase format.</returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }
    } 
}
