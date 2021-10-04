using System;

namespace Utilities
{
    public class Formatter
    {
        /// <summary>
        /// Get time properties by timer count
        /// </summary>
        /// <param name="timer"> Timer count </param>
        /// <param name="hours"> Hours to be calculated </param>
        /// <param name="minutes"> Minutes to be calculated </param>
        /// <param name="seconds"> Seconds to be calculated </param>
        private static void GetTime(long timer, out int hours, out int minutes, out int seconds)
        {
            hours = (int)(timer / 3600);
            minutes = (int)(timer - (hours * 3600)) / 60;
            seconds = (int)(timer - (hours * 3600) - (minutes * 60));
        }

        /// <summary>
        /// Get formatted ellapsed time in hours. Ex: "01:32:05"
        /// </summary>
        /// <param name="timer"> Timer count </param>
        /// <returns> Formatted ellapsed time </returns>
        public static string GetEllapsedTimeInHours(long timer)
        {
            int hours, minutes, seconds;
            GetTime(timer, out hours, out minutes, out seconds);
            return string.Format("{0}:{1}:{2}", hours.ToString("00"), minutes.ToString("00"), seconds.ToString("00"));
        }

        /// <summary>
        /// Get formatted ellapsed time in minutes. Ex: "10:32"
        /// </summary>
        /// <param name="timer"> Timer count </param>
        /// <returns> Formatted ellapsed time </returns>
        public static string GetEllapsedTimeInMinutes(int timer)
        {
            int hours, minutes, seconds;
            GetTime(timer, out hours, out minutes, out seconds);
            return string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
        }

        /// <summary>
        /// Get formatted value in currency. Ex: "1.324.894.9830"
        /// </summary>
        /// <param name="value"> Value to be formatted </param>
        /// <returns> Formatted value </returns>
        public static string FormatToCurrency(long value) => value.ToString("#,###,###,###");

        /// <summary>
        /// Format date timer to desired format
        /// </summary>
        /// <param name="timer"> Date timer </param>
        /// <param name="format"> Desired Format Mask </param>
        /// <returns>Formatted date timer </returns>
        public static string FormatDateTimer(long timer, string format) => DateTimeOffset.FromUnixTimeSeconds(timer).ToLocalTime().ToString(format);
    }
}
