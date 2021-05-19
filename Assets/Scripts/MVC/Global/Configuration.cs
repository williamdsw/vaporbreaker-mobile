using UnityEngine;

namespace MVC.Global
{
    public class Configuration
    {
        public class Queries
        {
            public class Level
            {
                public static string GetById => string.Concat(ListAll, " WHERE id = {0} ");
                public static string GetLastLevel => string.Concat(ListAll, " WHERE id = (SELECT MAX(id) FROM level) ");
                public static string ListAll => " SELECT id, name, is_unlocked, is_completed FROM level ";
                public static string ResetLevels => " UPDATE level SET is_unlocked = 0, is_completed = 0; UPDATE level SET is_unlocked = 1 WHERE id = (SELECT MIN(id) AS id FROM level); ";
                public static string UpdateFieldById => " UPDATE level {0} WHERE id = {1}; ";
            }

            public class Scoreboard
            {
                public static string DeleteAll => " DELETE FROM scoreboard; ";
                public static string GetByMaxScoreByLevel => " SELECT id, level_id, MAX(score) as score, MIN(time_score) as time_score, moment FROM scoreboard WHERE level_id = {0}; ";
                public static string Insert => " INSERT INTO scoreboard (level_id, score, time_score, moment) VALUES ({0}, {1}, {2}, {3}); ";
                public static string ListByLevel => " SELECT id, level_id, score, time_score, moment FROM scoreboard WHERE level_id = {0} ORDER BY score DESC; ";
            }

            public class Localization
            {
                public static string GetByLanguage => " SELECT id, language, content FROM localization WHERE language = '{0}'; ";
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