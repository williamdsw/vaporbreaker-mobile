using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ProgressManager
{
    private static string fileName = "/SaveProgress.dat";
    private static string filePath = string.Concat (Application.persistentDataPath, fileName);

    //--------------------------------------------------------------------------------//

    // Verifies file progress
    public static bool HasProgress ()
    {
        return File.Exists (filePath);
    }

    // Verifies the progress file and load data
    public static PlayerProgress LoadProgress ()
    {
        PlayerProgress progress = new PlayerProgress ();

        if (HasProgress ())
        {
            using (FileStream fileStream = File.Open (filePath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter ();
                progress = (PlayerProgress) binaryFormatter.Deserialize (fileStream);
            }
        }

        return progress;
    }

    // Create / Override save file
    public static void SaveProgress (PlayerProgress progress)
    {
        using (FileStream fileStream = File.Create (filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter ();
            binaryFormatter.Serialize (fileStream, progress);
        }
    }

    // Deletes save file
    public static void DeleteProgress ()
    {
        if (HasProgress ())
        {
            File.Delete (filePath);
        }
    }
}