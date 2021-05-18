using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.DAO
{
    public class LevelDAO : Connection
    {
        public LevelDAO() : base() { }

        public bool UpdateFieldById(long id, string subQuery)
        {
            try
            {
                return ExecuteNonQuery(string.Format(Configuration.Queries.Level.UpdateFieldById, subQuery, id)) == 1;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("LevelDAO::UpdateFieldById -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<Level> ListAll()
        {
            try
            {
                return Factory<Level>.CreateMany(ExecuteQuery(Configuration.Queries.Level.ListAll));
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("LevelDAO::ListAll -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Level GetById(long id)
        {
            try
            {
                if (id <= 0) throw new Exception(string.Format("Invalid Level Id = {0}", id));
                return Factory<Level>.CreateOne(ExecuteQuery(string.Format(Configuration.Queries.Level.GetById, id)));
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("LevelDAO::GetById -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Level GetLastLevel()
        {
            try
            {
                return Factory<Level>.CreateOne(ExecuteQuery(Configuration.Queries.Level.GetLastLevel));
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("LevelDAO::GetById -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void ResetLevels()
        {
            try
            {
                ExecuteNonQuery(Configuration.Queries.Level.ResetLevels);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("LevelDAO::ResetLevels -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}