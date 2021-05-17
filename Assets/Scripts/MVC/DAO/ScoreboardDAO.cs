using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.DAO
{
    public class ScoreboardDAO : Connection
    {
        public ScoreboardDAO() : base() { }

        public bool Insert(Scoreboard model)
        {
            try
            {
                string query = string.Format(Configuration.Queries.Scoreboard.Insert, model.LevelId, model.Score, model.TimeScore, model.Moment);
                return ExecuteNonQuery(query) == 1;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("ScoreboardDAO::Insert -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

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
                Debug.LogErrorFormat("ScoreboardDAO::ListByLevel -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Scoreboard GetByMaxScoreByLevel(long levelId)
        {
            try
            {
                string query = string.Format(Configuration.Queries.Scoreboard.GetByMaxScoreByLevel, levelId);
                return Factory<Scoreboard>.CreateOne(ExecuteQuery(query));
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("ScoreboardDAO::GetByMaxScoreByLevel -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteAll()
        {
            try
            {
                return ExecuteNonQuery(Configuration.Queries.Scoreboard.DeleteAll) != 0;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("ScoreboardDAO::DeleteAll -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}