using System;
using System.Globalization;
using System.Text;

namespace TW.Utility.Extension
{
    public static class ATimeSpanExtension
    {
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
        /// This method extends the TimeSpan class and converts a given TimeSpan object to a human-readable string format that includes days, hours, minutes, and seconds.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan object to convert to a readable string format.</param>
        /// <returns></returns>
        public static string ToStringShort(this TimeSpan timeSpan)
        {
            StringBuilder sb = new StringBuilder();
            if (timeSpan.TotalSeconds < 1) return sb.ToString();
            if (timeSpan.Days > 0) sb.Append($"{timeSpan.Days}:");
            if (timeSpan.Hours > 0 || !string.IsNullOrWhiteSpace(sb.ToString()))
                sb.Append(string.IsNullOrWhiteSpace(sb.ToString()) ? $"{timeSpan.Hours}:" : $"{timeSpan.Hours:00}:");
            if (timeSpan.Minutes > 0 || !string.IsNullOrWhiteSpace(sb.ToString()))
                sb.Append(string.IsNullOrWhiteSpace(sb.ToString())
                    ? $"{timeSpan.Minutes}:"
                    : $"{timeSpan.Minutes:00}:");
            if (timeSpan.Seconds > 0 || !string.IsNullOrWhiteSpace(sb.ToString()))
                sb.Append(string.IsNullOrWhiteSpace(sb.ToString()) ? $"{timeSpan.Seconds}" : $"{timeSpan.Seconds:00}");
            return sb.ToString();
        }

        /// <summary>
        /// This method extends the TimeSpan class and converts a given TimeSpan object to a human-readable string format that includes days, hours, minutes, and seconds.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan object to convert to a readable string format.</param>
        /// <returns></returns>
        public static string ToStringFull(this TimeSpan timeSpan)
        {
            StringBuilder sb = new StringBuilder();
            if (timeSpan.TotalSeconds < 1) return sb.ToString();
            if (timeSpan.Days > 0) sb.Append($"{timeSpan.Days}d ");
            if (timeSpan.Hours > 0 || !string.IsNullOrWhiteSpace(sb.ToString()))
                sb.Append(string.IsNullOrWhiteSpace(sb.ToString()) ? $"{timeSpan.Hours}h " : $"{timeSpan.Hours:00}h ");
            if (timeSpan.Minutes > 0 || !string.IsNullOrWhiteSpace(sb.ToString()))
                sb.Append(string.IsNullOrWhiteSpace(sb.ToString())
                    ? $"{timeSpan.Minutes}m "
                    : $"{timeSpan.Minutes:00}m ");
            if (timeSpan.Seconds > 0 || !string.IsNullOrWhiteSpace(sb.ToString()))
                sb.Append(string.IsNullOrWhiteSpace(sb.ToString())
                    ? $"{timeSpan.Seconds}s"
                    : $"{timeSpan.Seconds:00}s");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the default DateTime value formatted as a string using invariant culture.
        /// </summary>
        /// <returns>A string representation of the default DateTime.</returns>
        public static string DateTimeDefaultString()
        {
            return ((DateTime)default).DateTimeToString();
        }

        /// <summary>
        /// Converts a DateTime object to its string representation using invariant culture.
        /// </summary>
        /// <param name="dateTime">The DateTime value to convert.</param>
        /// <returns>A string representation of the DateTime.</returns>
        public static string DateTimeToString(this DateTime dateTime)
        {
            return dateTime.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Attempts to parse a string into a DateTime using invariant culture.
        /// </summary>
        /// <param name="dateTimeString">The string representation of a DateTime.</param>
        /// <returns>
        /// A parsed DateTime if successful; otherwise, the default DateTime value.
        /// </returns>
        public static DateTime StringToDateTime(this string dateTimeString)
        {
            return DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime time)
                ? time
                : default;
        }

        /// <summary>
        /// Converts a TimeSpan to a compact UI-friendly string format using day, hour, minute, or second units.
        /// Prioritizes the largest non-zero unit and includes the next smaller unit if applicable.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to format.</param>
        /// <returns>A short UI string (e.g., "2d05h", "3h20m", "4m10s", "30s").</returns>
        public static string TimeSpanToStringUI(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
            {
                return timeSpan.Hours > 0
                    ? $"{(int)timeSpan.TotalDays}d{timeSpan.Hours:D2}h"
                    : $"{(int)timeSpan.TotalDays}d";
            }

            if (timeSpan.TotalHours >= 1)
            {
                return timeSpan.Minutes > 0
                    ? $"{(int)timeSpan.TotalHours}h{timeSpan.Minutes:D2}m"
                    : $"{(int)timeSpan.TotalHours}h";
            }

            if (timeSpan.TotalMinutes >= 1)
            {
                return timeSpan.TotalSeconds > 0
                    ? $"{(int)timeSpan.TotalMinutes}m{timeSpan.Seconds:D2}s"
                    : $"{(int)timeSpan.TotalMinutes}m";
            }

            return $"{(int)timeSpan.TotalSeconds}s";
        }

        /// <summary>
        /// Converts a TimeSpan to a UI-friendly string format using colon separators between units.
        /// Prioritizes the largest non-zero unit and includes the next smaller unit if applicable.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to format.</param>
        /// <returns>A formatted string with colon separators (e.g., "2d:05h", "3h:20m", "4m:10s", "30s").</returns>
        public static string TimeSpanToStringUIWithColon(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
            {
                return timeSpan.Hours > 0
                    ? $"{(int)timeSpan.TotalDays}:d{timeSpan.Hours:D2}h"
                    : $"{(int)timeSpan.TotalDays}d";
            }

            if (timeSpan.TotalHours >= 1)
            {
                return timeSpan.Minutes > 0
                    ? $"{(int)timeSpan.TotalHours}h:{timeSpan.Minutes:D2}m"
                    : $"{(int)timeSpan.TotalHours}h";
            }

            if (timeSpan.TotalMinutes >= 1)
            {
                return timeSpan.TotalSeconds > 0
                    ? $"{(int)timeSpan.TotalMinutes}m:{timeSpan.Seconds:D2}s"
                    : $"{(int)timeSpan.TotalMinutes}m";
            }

            return $"{(int)timeSpan.TotalSeconds}s";
        }
    }
}