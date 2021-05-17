using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utilities
{
    public class ProgressManager
    {
        private static string fileName = "/SaveProgress.dat";
        private static string filePath = string.Concat(Application.persistentDataPath, fileName);

        public static bool HasProgress() => FileManager.Exists(filePath);

        public static PlayerProgress LoadProgress()
        {
            PlayerProgress progress = new PlayerProgress();

            if (HasProgress())
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    progress = (PlayerProgress)binaryFormatter.Deserialize(fileStream);
                }
            }

            return progress;
        }

        public static void SaveProgress(PlayerProgress progress)
        {
            try
            {
                using (FileStream fileStream = File.Create(filePath))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, progress);
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("ProgressManager::SaveProgress -> {0}", ex.Message);
                throw ex;
            }
        }
    }
}