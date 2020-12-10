
public class Formatter
{
    public static string FormatEllapsedTime (int timer)
    {
        int hours = timer / 3600;
        int minutes = (timer - (hours * 3600)) / 60;
        int seconds = timer - (hours * 3600) - (minutes * 60);
        return string.Concat (hours.ToString ("00"), ":", minutes.ToString ("00"), ":", seconds.ToString ("00"));
    }

    public static string FormatLevelName (string levelName)
    {
        // Cancels
        if (string.IsNullOrEmpty (levelName) || string.IsNullOrWhiteSpace (levelName)) { return null; }
        string newName = levelName.Replace ("_", " ");
        return newName;
    }

    public static string FormatToCurrency (int value)
    {
        return value.ToString ("#,###,###,###");
    }
}