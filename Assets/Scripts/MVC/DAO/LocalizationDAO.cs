using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using System.Collections.Generic;

namespace MVC.DAO
{
    /// <summary>
    /// Localization related queries
    /// </summary>
    public class LocalizationDAO : Connection
    {
        public LocalizationDAO() : base() { }

        /// <summary>
        /// Get a localization by language
        /// </summary>
        /// <param name="language"> Desired language </param>
        /// <returns> Localization instance </returns>
        public Localization GetByLanguage(string language)
        {
            try
            {
                string query = string.Format(Configuration.Queries.Localization.GetByLanguage, language);
                return Factory<Localization>.CreateOne(ExecuteQuery(query));
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

        /// <summary>
        /// Get all languages
        /// </summary>
        /// <returns> List of Localization instances </returns>
        public List<Localization> ListAll()
        {
            try
            {
                return Factory<Localization>.CreateMany(ExecuteQuery(Configuration.Queries.Localization.ListAll));
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