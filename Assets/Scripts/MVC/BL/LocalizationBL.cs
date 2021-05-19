using MVC.DAO;
using MVC.Models;

namespace MVC.BL
{
    public class LocalizationBL
    {
        private LocalizationDAO localizationDAO = new LocalizationDAO();

        public Localization GetByLanguage(string language) => localizationDAO.GetByLanguage(language);
    }
}