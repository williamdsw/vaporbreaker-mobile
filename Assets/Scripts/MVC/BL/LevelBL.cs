using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    /// <summary>
    /// Business Layer for Level
    /// </summary>
    public class LevelBL
    {
        private LevelDAO levelDAO = new LevelDAO();

        /// <summary>
        /// Update is level unlocked by Id
        /// </summary>
        /// <param name="id"> Level Id </param>
        /// <param name="isUnlocked"> Is unlocked ? </param>
        /// <returns> Query was updated ? </returns>
        public bool UpdateIsUnlockedById(long id, bool isUnlocked) => UpdateFieldById(id, string.Format(" SET is_unlocked = {0} ", (isUnlocked ? 1 : 0)));

        /// <summary>
        /// Update is completed by Id
        /// </summary>
        /// <param name="id"> Level Id </param>
        /// <param name="isCompleted"> Is completed ? </param>
        /// <returns> Query was updated ? </returns>
        public bool UpdateIsCompletedById(long id, bool isCompleted) => UpdateFieldById(id, string.Format(" SET is_completed = {0} ", (isCompleted ? 1 : 0)));

        /// <summary>
        /// Update a field by Id
        /// </summary>
        /// <param name="id"> Level Id </param>
        /// <param name="subQuery"> SubQuery with field </param>
        /// <returns> Query was updated ? </returns>
        private bool UpdateFieldById(long id, string subQuery) => levelDAO.UpdateFieldById(id, subQuery);

        /// <summary>
        /// List all levels
        /// </summary>
        /// <returns> List of Level instances </returns>
        public List<Level> ListAll() => levelDAO.ListAll();

        /// <summary>
        /// Get a level by Id
        /// </summary>
        /// <param name="id"> Level Id </param>
        /// <returns> Level instance </returns>
        public Level GetById(long id) => levelDAO.GetById(id);

        /// <summary>
        /// Get Last Level
        /// </summary>
        /// <returns> Level instance </returns>
        public Level GetLastLevel() => levelDAO.GetLastLevel();

        /// <summary>
        /// Reset all Levels data
        /// </summary>
        public void ResetLevels() => levelDAO.ResetLevels();
    }
}