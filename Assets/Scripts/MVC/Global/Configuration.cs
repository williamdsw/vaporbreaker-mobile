using System.IO;
using UnityEngine;

namespace MVC.Global
{
    /// <summary>
    /// Configuration settings
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Queries used in database
        /// </summary>
        public class Queries
        {
            public class Level
            {
                /// <summary>
                /// Get a level by Id
                /// </summary>
                public static string GetById => string.Concat(ListAll, " WHERE id = {0} ");

                /// <summary>
                /// Get Last Level
                /// </summary>
                public static string GetLastLevel => string.Concat(ListAll, " WHERE id = (SELECT MAX(id) FROM level) ");

                /// <summary>
                /// List all levels
                /// </summary>
                public static string ListAll => " SELECT * FROM level ";

                /// <summary>
                /// Reset all Levels data
                /// </summary>
                public static string ResetLevels => " UPDATE level SET is_unlocked = 0, is_completed = 0; UPDATE level SET is_unlocked = 1 WHERE id = (SELECT MIN(id) AS id FROM level); ";

                /// <summary>
                /// Update a field by Id
                /// </summary>
                public static string UpdateFieldById => " UPDATE level {0} WHERE id = {1}; ";
            }

            public class Localization
            {
                /// <summary>
                /// Get a localization by language
                /// </summary>
                public static string GetByLanguage => string.Concat(ListAll, " WHERE language = '{0}'; ");

                /// <summary>
                /// Get all languages
                /// </summary>
                public static string ListAll => " SELECT * FROM localization ";
            }

            public class Scoreboard
            {
                /// <summary>
                /// Delete all scoreboards
                /// </summary>
                public static string DeleteAll => " DELETE FROM scoreboard; ";

                /// <summary>
                /// Get scoreboard by max score and level id
                /// </summary>
                public static string GetByMaxScoreByLevel => " SELECT id, level_id, MAX(score) as score, MIN(time_score) as time_score, MAX(best_combo) as best_combo, MAX(moment) AS moment FROM scoreboard WHERE level_id = {0}; ";

                /// <summary>
                /// Insert scoreboard data
                /// </summary>
                public static string Insert => " INSERT INTO scoreboard (level_id, score, time_score, best_combo, moment) VALUES ({0}, {1}, {2}, {3}, {4}); ";

                /// <summary>
                /// List scoreboards by level
                /// </summary>
                public static string ListByLevel => " SELECT * FROM scoreboard WHERE level_id = {0} ORDER BY score DESC; ";
            }

            public class Track
            {
                /// <summary>
                /// List all tracks
                /// </summary>
                public static string ListAll => " SELECT * FROM track ";
            }
        }

        public class Properties
        {
            /// <summary>
            /// Database file name
            /// </summary>
            public static string DatabaseName => "database.s3db";

            /// <summary>
            /// Copied database path
            /// </summary>
            public static string DatabasePath => string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

            /// <summary>
            /// Local Streaming Assets folder path
            /// </summary>
            public static string DatabaseStreamingAssetsPath => string.Format("{0}/StreamingAssets/{1}", Application.dataPath, DatabaseName);

#if UNITY_ANDROID

            /// <summary>
            /// Local database path for Android
            /// </summary>
            public static string MobileDatabasePath => string.Format("jar:file://{0}!/assets/{1}", Application.dataPath, DatabaseName);
#elif UNITY_IOS

            /// <summary>
            /// Local database path for iOS
            /// </summary>
            public static string MobileDatabasePath => string.Format("file://{0}/Raw/{1}", Application.dataPath, DatabaseName);
#else

            /// <summary>
            /// Local database path for Others
            /// </summary>
            public static string MobileDatabasePath => string.Empty;
            
#endif

            /// <summary>
            /// Save progress path
            /// </summary>
            public static string ProgressPath => Path.Combine(Application.persistentDataPath, "SaveProgress.dat");
        }
    }
}