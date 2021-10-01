using Controllers.Core;
using MVC.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Panel
{
    /// <summary>
    /// Controller for Configuration Panel
    /// </summary>
    public class ConfigurationPanelController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button bgmVolumeButton;
        [SerializeField] private Button meVolumeButton;
        [SerializeField] private Button sfxVolumeButton;
        [SerializeField] private Button languageButton;
        [SerializeField] private Button aboutButton;
        [SerializeField] private Button resetProgressButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;

        // || Cached

        private Image bgmVolumeButtonImage;
        private Image meVolumeButtonImage;
        private Image sfxVolumeButtonImage;
        private TextMeshProUGUI bgmVolumeButtonLabel;
        private TextMeshProUGUI meVolumeButtonLabel;
        private TextMeshProUGUI sfxVolumeButtonLabel;
        private TextMeshProUGUI aboutButtonText;
        private TextMeshProUGUI resetProgressButtonText;
        private TextMeshProUGUI backButtonText;

        // || State

        private bool isBgmMuted = false;
        private bool isMeMuted = false;
        private bool isSfxMuted = false;

        // || Properties

        public static ConfigurationPanelController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GetRequiredComponents();
            Translate();
            BindEventListeners();
            LoadSettings();
        }

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                bgmVolumeButtonImage = bgmVolumeButton.transform.GetChild(0).GetComponent<Image>();
                meVolumeButtonImage = meVolumeButton.transform.GetChild(0).GetComponentInChildren<Image>();
                sfxVolumeButtonImage = sfxVolumeButton.transform.GetChild(0).GetComponentInChildren<Image>();
                bgmVolumeButtonLabel = bgmVolumeButton.GetComponentInChildren<TextMeshProUGUI>();
                meVolumeButtonLabel = meVolumeButton.GetComponentInChildren<TextMeshProUGUI>();
                sfxVolumeButtonLabel = sfxVolumeButton.GetComponentInChildren<TextMeshProUGUI>();
                //languageButtonText = languageButton.GetComponentInChildren<TextMeshProUGUI>(); !TODO
                aboutButtonText = aboutButton.GetComponentInChildren<TextMeshProUGUI>();
                resetProgressButtonText = resetProgressButton.GetComponentInChildren<TextMeshProUGUI>();
                backButtonText = backButton.GetComponentInChildren<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        private void Translate()
        {
            try
            {
                titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_configurations);
                bgmVolumeButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_backgroundmusic);
                meVolumeButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_musiceffects);
                sfxVolumeButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_soundeffects);
                //languageButton.text = LocalizationController.Instance.GetWord(LocalizationFields.general_about); !TODO
                aboutButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_about);
                resetProgressButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_resetprogress);
                backButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_back);
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
                bgmVolumeButton.onClick.AddListener(() =>
                {
                    isBgmMuted = !isBgmMuted;
                    ToggleAudio(isBgmMuted, AudioController.Instance.AudioSourceBGM, bgmVolumeButtonImage);
                });

                meVolumeButton.onClick.AddListener(() =>
                {
                    isMeMuted = !isMeMuted;
                    ToggleAudio(isMeMuted, AudioController.Instance.AudioSourceME, meVolumeButtonImage);
                });

                sfxVolumeButton.onClick.AddListener(() =>
                {
                    isSfxMuted = !isSfxMuted;
                    ToggleAudio(isSfxMuted, AudioController.Instance.AudioSourceSFX, sfxVolumeButtonImage);
                });

                /*languageButton.onClick.AddListener(() => !TODO
                {
                    TogglePanel(false);
                    aboutPanel.SetActive(true);
                });*/

                aboutButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    AboutPanelController.Instance.TogglePanel(true);
                });

                resetProgressButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    ConfirmPanelController.Instance.TogglePanel(true);
                });

                backButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    SelectLevelsPanelController.Instance.TogglePanel(true);
                    SaveSettings();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Show or hide the panel
        /// </summary>
        /// <param name="toShow"> Is to show the panel ? </param>
        public void TogglePanel(bool toShow)
        {
            panel.SetActive(toShow);

            if (toShow)
            {
                Translate();
            }
        }

        /// <summary>
        /// Toggle audio by button
        /// </summary>
        /// <param name="flagIndex"> Index of Flag </param>
        /// <param name="audioSource"> AudioSource to be muted / unmuted </param>
        /// <param name="buttonImage"> Image button to be changed </param>
        private void ToggleAudio(bool flag, AudioSource audioSource, Image buttonImage)
        {
            audioSource.mute = flag;
            buttonImage.sprite = (flag ? soundOffSprite : soundOnSprite);
        }

        /// <summary>
        /// Reset values to default
        /// </summary>
        private void SetDefaultValues()
        {
            isBgmMuted = isMeMuted = isSfxMuted = false;

            UpdateUI();
            ApplyValues();
        }

        /// <summary>
        /// Apply selected values
        /// </summary>
        private void ApplyValues()
        {
            AudioController.Instance.AudioSourceBGM.mute = isBgmMuted;
            AudioController.Instance.AudioSourceME.mute = isMeMuted;
            AudioController.Instance.AudioSourceSFX.mute = isSfxMuted;
        }

        /// <summary>
        /// Update UI elements
        /// </summary>
        private void UpdateUI()
        {
            try
            {
                bgmVolumeButtonImage.sprite = (isBgmMuted ? soundOffSprite : soundOnSprite);
                meVolumeButtonImage.sprite = (isMeMuted ? soundOffSprite : soundOnSprite);
                sfxVolumeButtonImage.sprite = (isSfxMuted ? soundOffSprite : soundOnSprite);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load settings from PlayerPrefs
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                if (!PlayerPrefsController.HasPlayerPrefs)
                {
                    SetDefaultValues();
                    return;
                }

                isBgmMuted = (!string.IsNullOrEmpty(PlayerPrefsController.IsBackgroundMusicMute) ? bool.Parse(PlayerPrefsController.IsBackgroundMusicMute) : false);
                isMeMuted = (!string.IsNullOrEmpty(PlayerPrefsController.IsMusicEffectsMute) ? bool.Parse(PlayerPrefsController.IsMusicEffectsMute) : false);
                isSfxMuted = (!string.IsNullOrEmpty(PlayerPrefsController.IsSoundEffectsMute) ? bool.Parse(PlayerPrefsController.IsSoundEffectsMute) : false);

                UpdateUI();
                ApplyValues();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save settings to PlayerPrefs
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                PlayerPrefsController.IsBackgroundMusicMute = isBgmMuted.ToString();
                PlayerPrefsController.IsMusicEffectsMute = isMeMuted.ToString();
                PlayerPrefsController.IsSoundEffectsMute = isSfxMuted.ToString();
                PlayerPrefsController.HasPlayerPrefs = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}