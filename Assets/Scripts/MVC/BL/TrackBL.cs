using MVC.DAO;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.BL
{
    public class TrackBL
    {
        private TrackDAO trackDAO = new TrackDAO();

        public List<Track> ListAll() => trackDAO.ListAll();
    }
}