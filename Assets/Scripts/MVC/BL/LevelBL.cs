using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    public class LevelBL
    {
        private LevelDAO levelDAO = new LevelDAO();

        public List<Level> ListAll() => levelDAO.ListAll();
    }
}