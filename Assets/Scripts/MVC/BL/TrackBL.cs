using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    /// <summary>
    /// Business Layer for Scoreboard
    /// </summary>
    public class TrackBL
    {
        /// <summary>
        /// List all tracks
        /// </summary>
        /// <returns> List of Track instances </returns>
        public List<Track> ListAll() => new TrackDAO().ListAll();
    }
}