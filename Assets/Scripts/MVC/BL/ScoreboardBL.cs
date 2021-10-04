using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    /// <summary>
    /// Business Layer for Scoreboard
    /// </summary>
    public class ScoreboardBL
    {
        private ScoreboardDAO scoreboardDAO = new ScoreboardDAO();

        /// <summary>
        /// Insert scoreboard data
        /// </summary>
        /// <param name="model"> Instance of Scoreboard </param>
        /// <returns> Scoreboard was inserted ? </returns>
        public bool Insert(Scoreboard model) => scoreboardDAO.Insert(model);

        /// <summary>
        /// List scoreboards by level
        /// </summary>
        /// <param name="levelId"> Level Id </param>
        /// <returns> List of Scoreboard instances </returns>
        public List<Scoreboard> ListByLevel(long levelId) => scoreboardDAO.ListByLevel(levelId);

        /// <summary>
        /// Get scoreboard by max score and level id
        /// </summary>
        /// <param name="levelId"> Level Id </param>
        /// <returns> Scoreboard instance </returns>
        public Scoreboard GetByMaxScoreByLevel(long levelId) => scoreboardDAO.GetByMaxScoreByLevel(levelId);

        /// <summary>
        /// Delete all scoreboards
        /// </summary>
        /// <returns> All scoreboards were deleted ? </returns>
        public bool DeleteAll() => scoreboardDAO.DeleteAll();
    }
}