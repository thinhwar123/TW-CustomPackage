using System;
using System.Linq;

namespace TW.Utility.Extension
{
    public static class AFlagEnumExtension
	{
        public static bool ContainsAtLeastOne<T>(this T origin, T value) where T : Enum
        {
            int originValue = Convert.ToInt32(origin);
            int valueValue = Convert.ToInt32(value);
            return (originValue & valueValue) != 0;
        }
        public static bool ContainsAll<T>(this T origin, T value) where T : Enum
        {
            int originValue = Convert.ToInt32(origin);
            int valueValue = Convert.ToInt32(value);
            return (originValue & valueValue) == valueValue;
        }

        public static bool ContainsOnly<T>(this T origin, T value) where T : Enum
        {
            int originValue = Convert.ToInt32(origin);
            int valueValue = Convert.ToInt32(value);
            return originValue == valueValue;
        }
        public static bool ContainsAtLeastOne<T>(this T origin, params T[] value) where T : Enum
        {
            int originValue = Convert.ToInt32(origin);
            int combinedValue = value.Select(val => Convert.ToInt32(val)).Aggregate(0, (current, valValue) => current | valValue);
            return (originValue & combinedValue) != 0;
        }
        public static bool ContainsAll<T>(this T origin, params T[] value) where T : Enum
        {
            int originValue = Convert.ToInt32(origin);
            int combinedValue = value.Select(val => Convert.ToInt32(val)).Aggregate(0, (current, valValue) => current | valValue);
            return (originValue & combinedValue) == combinedValue;
        }
        public static bool ContainsOnly<T>(this T origin, params T[] value) where T : Enum
        {
            int originValue = Convert.ToInt32(origin);
            int combinedValue = value.Select(val => Convert.ToInt32(val)).Aggregate(0, (current, valValue) => current | valValue);
            return originValue == combinedValue;
        }
    }

}