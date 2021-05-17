
namespace Utilities
{
    public class Formatter
    {
        public static string FormatEllapsedTime(long timer)
        {
            long hours = (timer / 3600);
            long minutes = (timer - (hours * 3600)) / 60;
            long seconds = timer - (hours * 3600) - (minutes * 60);
            return string.Concat(hours.ToString("00"), ":", minutes.ToString("00"), ":", seconds.ToString("00"));
        }

        public static string FormatLevelName(string levelName)
        {
            // Cancels
            if (string.IsNullOrEmpty(levelName) || string.IsNullOrWhiteSpace(levelName)) return string.Empty;
            return levelName.Replace("_", string.Empty).Replace("Level", string.Empty);
        }

        public static string FormatToCurrency(long value) => value.ToString("#,###,###,###");
    }
}
