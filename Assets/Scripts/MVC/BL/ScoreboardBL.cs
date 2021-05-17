using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    public class ScoreboardBL
    {
        private ScoreboardDAO scoreboardDAO = new ScoreboardDAO();

        public bool Insert(Scoreboard model) => scoreboardDAO.Insert(model);

        public List<Scoreboard> ListByLevel(long levelId) => scoreboardDAO.ListByLevel(levelId);

        public Scoreboard GetByMaxScoreByLevel(long levelId) => scoreboardDAO.GetByMaxScoreByLevel(levelId);

        public bool DeleteAll() => scoreboardDAO.DeleteAll();
    }
}