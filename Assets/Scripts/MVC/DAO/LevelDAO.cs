using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using System.Collections.Generic;

namespace MVC.DAO
{
    /// <summary>
    /// Level related queries
    /// </summary>
    public class LevelDAO : Connection
    {
        public LevelDAO() : base() { }

        /// <summary>
        /// Update a field by Id
        /// </summary>
        /// <param name="id"> Level Id </param>
        /// <param name="subQuery"> SubQuery with field </param>
        /// <returns> Query was updated ? </returns>
        public bool UpdateFieldById(long id, string subQuery)
        {
            try
            {
                return ExecuteNonQuery(string.Format(Configuration.Queries.Level.UpdateFieldById, subQuery, id)) == 1;
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
        /// List all levels
        /// </summary>
        /// <returns> List of Level instances </returns>
        public List<Level> ListAll()
        {
            try
            {
                return Factory<Level>.CreateMany(ExecuteQuery(Configuration.Queries.Level.ListAll));
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
        /// Get a level by Id
        /// </summary>
        /// <param name="id"> Level Id </param>
        /// <returns> Level instance </returns>
        public Level GetById(long id)
        {
            try
            {
                if (id <= 0) throw new Exception(string.Format("Invalid Level Id = {0}", id));
                return Factory<Level>.CreateOne(ExecuteQuery(string.Format(Configuration.Queries.Level.GetById, id)));
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
        /// Get Last Level
        /// </summary>
        /// <returns> Level instance </returns>
        public Level GetLastLevel()
        {
            try
            {
                return Factory<Level>.CreateOne(ExecuteQuery(Configuration.Queries.Level.GetLastLevel));
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
        /// Reset all Levels data
        /// </summary>
        public void ResetLevels()
        {
            try
            {
                ExecuteNonQuery(Configuration.Queries.Level.ResetLevels);
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