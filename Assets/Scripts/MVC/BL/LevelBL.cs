using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    public class LevelBL
    {
        private LevelDAO levelDAO = new LevelDAO();

        public bool UpdateIsUnlockedById(long id, bool isUnlocked) => UpdateFieldById(id, string.Format(" SET is_unlocked = {0} ", (isUnlocked ? 1 : 0)));
        public bool UpdateIsCompletedById(long id, bool isCompleted) => UpdateFieldById(id, string.Format(" SET is_completed = {0} ", (isCompleted ? 1 : 0)));
        private bool UpdateFieldById(long id, string subQuery) => levelDAO.UpdateFieldById(id, subQuery);
        public List<Level> ListAll() => levelDAO.ListAll();
        public Level GetById(long id) => levelDAO.GetById(id);
        public Level GetLastLevel() => levelDAO.GetLastLevel();
        public void ResetLevels() => levelDAO.ResetLevels();
    }
}