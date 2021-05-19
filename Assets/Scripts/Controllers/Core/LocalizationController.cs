using System;
using System.Collections.Generic;
using MVC.BL;
using MVC.Enums;
using MVC.Models;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;

namespace Controllers.Core
{
    public class LocalizationController : MonoBehaviour
    {
        private Dictionary<string, Dictionary<string, string>> dictionary;

        // || Properties

        public static LocalizationController Instance { get; private set; }

        public int DictionaryCount => dictionary.Count;

        private void Awake()
        {
            dictionary = new Dictionary<string, Dictionary<string, string>>();
            SetupSingleton();
        }

        private void SetupSingleton()
        {
            int numberOfInstances = FindObjectsOfType(GetType()).Length;
            if (numberOfInstances > 1)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void DefineLocalization()
        {
            string language = PlayerPrefsController.Language;
            string folderPath = string.Empty;
            if (string.IsNullOrEmpty(language) || string.IsNullOrWhiteSpace(language))
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English: default: language = SystemLanguage.English.ToString(); break;
                    case SystemLanguage.Portuguese: language = SystemLanguage.Portuguese.ToString(); break;
                }

                PlayerPrefsController.Language = language;
            }

            LocalizationBL localizationBL = new LocalizationBL();
            Localization localization = localizationBL.GetByLanguage(language);
            dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(localization.Content);
        }

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