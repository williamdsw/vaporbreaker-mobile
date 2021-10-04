using MVC.DAO;
using MVC.Models;

namespace MVC.BL
{
    /// <summary>
    /// Business Layer for Localization
    /// </summary>
    public class LocalizationBL
    {
        /// <summary>
        /// Get a localization by language
        /// </summary>
        /// <param name="language"> Desired language </param>
        /// <returns> Localization instance </returns>
        public Localization GetByLanguage(string language) => new LocalizationDAO().GetByLanguage(language);
    }
}