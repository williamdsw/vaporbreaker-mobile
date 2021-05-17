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
    }
}