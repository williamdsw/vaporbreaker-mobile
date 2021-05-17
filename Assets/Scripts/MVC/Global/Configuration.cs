using UnityEngine;

namespace MVC.Global
{
    public class Configuration
    {
        public class Queries
        {
            public class Level
            {
                public static string ListAll => " SELECT id, name FROM level; ";
            }

            public class Scoreboard
            {
                public static string DeleteAll => " DELETE FROM scoreboard;";
                public static string Insert => " INSERT INTO scoreboard (level_id, score, time_score, moment) VALUES ({0}, {1}, {2}, {3}); ";
                public static string ListByLevel => " SELECT id, score, timescore, moment FROM scoreboard WHERE level_id = {0} ORDER BY score DESC; ";
                public static string GetByMaxScoreByLevel => " SELECT id, score, timescore, moment FROM scoreboard WHERE score = (SELECT MAX(score) FROM scoreboard) AND level_id = {0}; ";
            }
        }

        public class Properties
        {
            public static string DatabaseName => "database.s3db";
            public static string DatabasePath => string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
            public static string DatabaseStreamingAssetsPath => string.Format("{0}/StreamingAssets/{1}", Application.dataPath, DatabaseName);

#if UNITY_ANDROID
            public static string MobileDatabasePath => string.Format("jar:file://{0}!/assets/{1}", Application.dataPath, DatabaseName);
#elif UNITY_IOS
            public static string MobileDatabasePath => string.Format("file://{0}/Raw/{1}", Application.dataPath, DatabaseName);
#else
            public static string MobileDatabasePath => string.Empty;
#endif
        }
    }
}