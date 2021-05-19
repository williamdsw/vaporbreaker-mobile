using MVC.Global;
using MVC.Models;
using MVC.Utils;
using System;
using UnityEngine;

namespace MVC.DAO
{
    public class LocalizationDAO : Connection
    {
        public LocalizationDAO() : base() { }

        public Localization GetByLanguage(string language)
        {
            try
            {
                string query = string.Format(Configuration.Queries.Localization.GetByLanguage, language);
                return Factory<Localization>.CreateOne(ExecuteQuery(query));
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("LocalizationDAO::Get -> {0}", ex.Message);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}