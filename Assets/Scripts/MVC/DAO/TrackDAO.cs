using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using System.Collections.Generic;

namespace MVC.DAO
{
    /// <summary>
    /// Track data
    /// </summary>
    public class TrackDAO : Connection
    {
        public TrackDAO() : base() { }

        /// <summary>
        /// List all tracks
        /// </summary>
        /// <returns> List of Track instances </returns>
        public List<Track> ListAll()
        {
            try
            {
                return Factory<Track>.CreateMany(ExecuteQuery(Configuration.Queries.Track.ListAll));
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