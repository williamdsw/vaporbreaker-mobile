using MVC.Global;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    /// <summary>
    /// Manager for Progress
    /// </summary>
    public class ProgressManager
    {
        /// <summary>
        /// Check if progress file exists
        /// </summary>
        /// <returns> true | false </returns>
        public static bool HasProgress() => FileManager.Exists(Configuration.Properties.ProgressPath);

        /// <summary>
        /// Load current or new progress
        /// </summary>
        /// <returns> Instance of PlayerProgress </returns>
        public static PlayerProgress LoadProgress()
        {
            PlayerProgress progress = new PlayerProgress();

            if (HasProgress())
            {
                using (FileStream fileStream = File.Open(Configuration.Properties.ProgressPath, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    progress = (PlayerProgress)binaryFormatter.Deserialize(fileStream);
                }
            }

            return progress;
        }

        /// <summary>
        /// Save current progress to file
        /// </summary>
        /// <param name="progress"> Instance of PlayerProgress </param>
        public static void SaveProgress(PlayerProgress progress)
        {
            try
            {
                using (FileStream fileStream = File.Create(Configuration.Properties.ProgressPath))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, progress);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete current progress
        /// </summary>
        public static void DeleteProgress()
        {
            try
            {
                if (HasProgress())
                {
                    FileManager.Delete(Configuration.Properties.ProgressPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}