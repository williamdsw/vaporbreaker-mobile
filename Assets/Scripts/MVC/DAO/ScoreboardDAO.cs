using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using System.Collections.Generic;

namespace MVC.DAO
{
    /// <summary>
    /// Scoreboard data
    /// </summary>
    public class ScoreboardDAO : Connection
    {
        public ScoreboardDAO() : base() { }

        /// <summary>
        /// Insert scoreboard data
        /// </summary>
        /// <param name="model"> Instance of Scoreboard </param>
        /// <returns> Scoreboard was inserted ? </returns>
        public bool Insert(Scoreboard model)
        {
            try
            {
                string query = string.Format(Configuration.Queries.Scoreboard.Insert, model.LevelId, model.Score, model.TimeScore, model.BestCombo, model.Moment);
                return ExecuteNonQuery(query) == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// List scoreboards by level
        /// </summary>
        /// <param name="levelId"> Level Id </param>
        /// <returns> List of Scoreboard instances </returns>
        public List<Scoreboard> ListByLevel(long levelId)
        {
            try
            {
                if (levelId <= 0) throw new Exception(string.Format("Invalid Level Id = {0}", levelId));
                string query = string.Format(Configuration.Queries.Scoreboard.ListByLevel, levelId);
                return Factory<Scoreboard>.CreateMany(ExecuteQuery(query));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Get scoreboard by max score and level id
        /// </summary>
        /// <param name="levelId"> Level Id </param>
        /// <returns> Scoreboard instance </returns>
        public Scoreboard GetByMaxScoreByLevel(long levelId)
        {
            try
            {
                string query = string.Format(Configuration.Queries.Scoreboard.GetByMaxScoreByLevel, levelId);
                return Factory<Scoreboard>.CreateOne(ExecuteQuery(query));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Delete all scoreboards
        /// </summary>
        /// <returns> All scoreboards were deleted ? </returns>
        public bool DeleteAll()
        {
            try
            {
                return ExecuteNonQuery(Configuration.Queries.Scoreboard.DeleteAll) != 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}