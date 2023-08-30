using System;

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
    }

}