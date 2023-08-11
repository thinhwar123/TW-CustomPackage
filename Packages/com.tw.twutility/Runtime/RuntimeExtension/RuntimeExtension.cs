namespace TW.Utility
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using System;
    using System.Globalization;
    using System.Linq;

    public static class RuntimeExtension
    {
        /// <summary>
        /// This is a public static boolean variable that can be used to check if the UI is currently being touched by the user.
        /// </summary>
        public static bool m_IsTouchingUI;
        /// <summary>
        /// This method checks if the mouse pointer or touch is currently over a UI element and returns a boolean value indicating whether it is or not.
        /// </summary>
        /// <returns></returns>

        public static bool IsPointerOverUIGameObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                {
                    m_IsTouchingUI = true;
                    return true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && m_IsTouchingUI)
            {
                m_IsTouchingUI = false;
                return true;
            }

            return m_IsTouchingUI;
        }
        /// <summary>
        /// This method extends the TimeSpan class and converts a given TimeSpan object to a human-readable string format that includes days, hours, minutes, and seconds.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan object to convert to a readable string format.</param>
        /// <returns></returns>
        public static string ToStringReadable(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds < 1) return "0s";
            return string.Format("{0}{1}{2}{3}",
                timeSpan.Days > 0 ? $"{timeSpan.Days}d " : "",
                timeSpan.Hours > 0 ? $"{timeSpan.Hours}h " : "",
                timeSpan.Minutes > 0 ? $"{timeSpan.Minutes}m " : "",
                timeSpan.Seconds > 0 ? $"{timeSpan.Seconds}s " : "");
        }
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
        /// Returns a new enumerable of count random elements from the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to select random elements from.</param>
        /// <param name="count">The number of random elements to select from the input enumerable.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> enumerable, int count)
        {
            return enumerable.Shuffle().ToList().GetRange(0, count);
        }
        /// <summary>
        /// Returns a new random elements from the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to select random elements from.</param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            IEnumerable<T> enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            return enumerable1.ElementAt(UnityEngine.Random.Range(0, enumerable1.Count()));
        }
        /// <summary>
        /// Randomly shuffles the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The input enumerable to shuffle.</param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            List<T> list = new(enumerable);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
            return list;
        }
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
        /// Sets the global scale of a transform while maintaining its current position and rotation.
        /// </summary>
        /// <param name="transform">The transform to set the global scale on.</param>
        /// <param name="globalScale">The desired global scale.</param>
        /// <returns>The original Transform object.</returns>
        public static Transform SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one; // reset local scale
            Vector3 lossyScale = transform.lossyScale;
            transform.localScale = new Vector3(
                globalScale.x / lossyScale.x,
                globalScale.y / lossyScale.y,
                globalScale.z / lossyScale.z
            );
            return transform;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T GetComponentInParentUntilNoParent<T>(this Component component) where T : Component
        {
            return component.GetComponentInParent<T>() ?? component.transform.parent?.GetComponentInParentUntilNoParent<T>();
        }
        /// <summary>
        /// Determines whether a screen point is contained within the boundaries of the RectTransform.
        /// </summary>
        /// <param name="rectTransform">The RectTransform to check.</param>
        /// <param name="screenPoint">The screen point to check.</param>
        /// <param name="includeChildren">Optional parameter to include children RectTransforms in the check. Default is false.</param>
        /// <returns>True if the screen point is contained within the RectTransform boundaries, false otherwise.</returns>
        public static bool ContainsScreenPoint(this RectTransform rectTransform, Vector2 screenPoint, bool includeChildren = false)
        {
            return !includeChildren
                ? RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint)
                : rectTransform.GetComponentsInChildren<RectTransform>().Any(x => x.ContainsScreenPoint(screenPoint));
        }
    } 
}
