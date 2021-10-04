using MVC.BL;
using MVC.Enums;
using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for Localization
    /// </summary>
    public class LocalizationController : MonoBehaviour
    {
        // || Cached

        private Dictionary<string, Dictionary<string, string>> dictionary;

        // || Properties

        public static LocalizationController Instance { get; private set; }

        public int DictionaryCount => dictionary.Count;

        private void Awake() => SetupSingleton();

        /// <summary>
        /// Setup singleton instance
        /// </summary>
        private void SetupSingleton()
        {
            try
            {
                if (FindObjectsOfType(GetType()).Length > 1)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get current or default localization
        /// </summary>
        public void GetLocalization()
        {
            try
            {
                string language = PlayerPrefsController.Language;
                string folderPath = string.Empty;
                if (string.IsNullOrEmpty(language) || string.IsNullOrWhiteSpace(language))
                {
                    switch (Application.systemLanguage)
                    {
                        case SystemLanguage.English: default: language = SystemLanguage.English.ToString(); break;
                        case SystemLanguage.Italian: language = SystemLanguage.Italian.ToString(); break;
                        case SystemLanguage.Portuguese: language = SystemLanguage.Portuguese.ToString(); break;
                        case SystemLanguage.Spanish: language = SystemLanguage.Spanish.ToString(); break;
                    }

                    PlayerPrefsController.Language = language;
                }

                LoadLocalization(language);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load localization by language
        /// </summary>
        /// <param name="language"> Desired language </param>
        public void LoadLocalization(string language)
        {
            try
            {
                Localization localization = new LocalizationBL().GetByLanguage(language);
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(localization.Content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get translated word by field
        /// </summary>
        /// <param name="field"> Desired field </param>
        /// <returns> Translated word </returns>
        public string GetWord(LocalizationFields field)
        {
            try
            {
                string[] keys = field.ToString().Split('_');
                return dictionary[keys[0]][keys[1]];
            }
            catch (Exception ex)
            {
                Debug.LogFormat("{0}\n{1}", ex.Message, field);
                return string.Empty;
            }
        }
    }
}