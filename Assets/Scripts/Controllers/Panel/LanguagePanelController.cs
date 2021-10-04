using Controllers.Core;
using MVC.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Panel
{
    /// <summary>
    /// Controller for Language Panel
    /// </summary>
    public class LanguagePanelController : MonoBehaviour
    {
        [Header("Required UI Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button usaFlagButton;
        [SerializeField] private Button brazilFlagButton;
        [SerializeField] private Button spainFlagButton;
        [SerializeField] private Button italyFlagButton;
        [SerializeField] private Button franceFlagButton;
        [SerializeField] private Button germanyFlagButton;
        [SerializeField] private Button russiaFlagButton;
        [SerializeField] private Button japanFlagButton;
        [SerializeField] private Button backButton;

        [Header("Other Text to Translate")]
        [SerializeField] private TextMeshProUGUI headerLabel;

        // || State

        private string currentLanguage;

        // || Cached

        private TextMeshProUGUI backButtonLabel;

        // || Properties

        public static LanguagePanelController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            GetRequiredComponents();
            Translate();
            BindEventListeners();
        }

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                backButtonLabel = backButton.GetComponentInChildren<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        public void Translate()
        {
            try
            {
                headerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_language);
                backButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_back);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind event listeners to elements
        /// </summary>
        private void BindEventListeners()
        {
            try
            {
                usaFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.English.ToString()));
                brazilFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.Portuguese.ToString()));
                spainFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.Spanish.ToString()));
                italyFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.Italian.ToString()));
                franceFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.French.ToString()));
                germanyFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.German.ToString()));
                russiaFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.Russian.ToString()));
                japanFlagButton.onClick.AddListener(() => SetLanguage(SystemLanguage.Japanese.ToString()));
                backButton.onClick.AddListener(() => SaveAndBackToConfigurationPanel());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set desired language
        /// </summary>
        /// <param name="language"> Desired system language </param>
        private void SetLanguage(string language)
        {
            try
            {
                currentLanguage = language;
                LocalizationController.Instance.LoadLocalization(currentLanguage);
                Translate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save current select language and get back to main menu
        /// </summary>
        private void SaveAndBackToConfigurationPanel()
        {
            PlayerPrefsController.Language = currentLanguage;
            TogglePanel(false);
            ConfigurationPanelController.Instance.TogglePanel(true);
        }

        /// <summary>
        /// Show or hide the panel
        /// </summary>
        /// <param name="toShow"> Is to show the panel ? </param>
        public void TogglePanel(bool toShow) => panel.SetActive(toShow);
    }
}