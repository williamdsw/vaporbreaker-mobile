using Controllers.Core;
using MVC.Enums;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Menu
{
    public class ConfigurationController : MonoBehaviour
    {
        // Config
        [Header("UI Elements")]
        [SerializeField] private GameObject configurationPanel;
        [SerializeField] private GameObject selectLevelsPanel;
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private GameObject confirmBox;
        [SerializeField] private Button[] volumeButtons;
        [SerializeField] private Button aboutButton;
        [SerializeField] private Button resetProgressButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;

        // || Cached

        private List<TextMeshProUGUI> volumeButtonsTexts;
        private TextMeshProUGUI aboutButtonText;
        private TextMeshProUGUI resetProgressButtonText;
        private TextMeshProUGUI backButtonText;

        // State
        private bool[] muteFlags = { false, false, false };
        private List<Image> volumeButtonImages = new List<Image>();
        private AudioSource[] sources;
        private string[] audioPrefs;

        private void Awake()
        {
            sources = new AudioSource[3]
            {
                AudioController.Instance.AudioSourceBGM,
                AudioController.Instance.AudioSourceME,
                AudioController.Instance.AudioSourceSFX,
            };

            audioPrefs = new string[3]
            {
                PlayerPrefsController.IsBackgroundMusicMute.ToString(),
                PlayerPrefsController.IsMusicEffectsMute.ToString(),
                PlayerPrefsController.IsSoundEffectsMute.ToString(),
            };

            volumeButtonsTexts = new List<TextMeshProUGUI>();
            for (int index = 0; index < volumeButtons.Length; index++)
            {
                volumeButtonsTexts.Add(volumeButtons[index].GetComponentInChildren<TextMeshProUGUI>());
            }

            aboutButtonText = aboutButton.GetComponentInChildren<TextMeshProUGUI>();
            resetProgressButtonText = resetProgressButton.GetComponentInChildren<TextMeshProUGUI>();
            backButtonText = backButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            TranslateLabels();
            GetReferences();
            BindClickEvents();
            LoadSettings();
        }

        private void GetReferences()
        {
            foreach (Button item in volumeButtons)
            {
                Transform child = item.transform.GetChild(0);
                volumeButtonImages.Add(child.GetComponent<Image>());
            }
        }

        private void TranslateLabels()
        {
            aboutButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_about);
            resetProgressButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.configurations_resetprogress);
            backButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_back);

            string[] words = 
            {
                LocalizationController.Instance.GetWord(LocalizationFields.configurations_backgroundmusic),
                LocalizationController.Instance.GetWord(LocalizationFields.configurations_musiceffects),
                LocalizationController.Instance.GetWord(LocalizationFields.configurations_soundeffects),
            };

            for (int index = 0; index < words.Length ; index++)
            {
                volumeButtonsTexts[index].text = words[index];
            }
        }

        private void BindClickEvents()
        {
            for (int index = 0; index < volumeButtons.Length; index++)
            {
                int keep = index;
                volumeButtons[index].onClick.AddListener(() => ToggleAudio(keep));
            }

            aboutButton.onClick.AddListener(() =>
            {
                configurationPanel.SetActive(false);
                aboutPanel.SetActive(true);
            });

            resetProgressButton.onClick.AddListener(() =>
            {
                configurationPanel.SetActive(false);
                confirmBox.SetActive(true);
            });

            backButton.onClick.AddListener(() =>
            {
                configurationPanel.SetActive(false);
                selectLevelsPanel.SetActive(true);
                SaveSettings();
            });
        }

        private void ToggleAudio(int index)
        {
            muteFlags[index] = !muteFlags[index];
            sources[index].mute = muteFlags[index];
            volumeButtonImages[index].sprite = (muteFlags[index] ? soundOffSprite : soundOnSprite);
        }

        private void DefaultValues()
        {
            for (int index = 0; index < muteFlags.Length; index++)
            {
                muteFlags[index] = false;
            }

            UpdateUI();
            ApplyValues();
        }

        private void ApplyValues()
        {
            for (int index = 0; index < muteFlags.Length; index++)
            {
                sources[index].mute = muteFlags[index];
            }
        }

        private void UpdateUI()
        {
            for (int index = 0; index < volumeButtonImages.Count; index++)
            {
                volumeButtonImages[index].sprite = (muteFlags[index] ? soundOffSprite : soundOnSprite);
            }
        }

        private void LoadSettings()
        {
            if (!PlayerPrefsController.HasPlayerPrefs)
            {
                DefaultValues();
                return;
            }

            for (int index = 0; index < muteFlags.Length; index++)
            {
                muteFlags[index] = (!string.IsNullOrEmpty(audioPrefs[index]) ? bool.Parse(audioPrefs[index]) : false);
            }

            UpdateUI();
            ApplyValues();
        }

        private void SaveSettings()
        {
            PlayerPrefsController.IsBackgroundMusicMute = muteFlags[0].ToString();
            PlayerPrefsController.IsMusicEffectsMute = muteFlags[1].ToString();
            PlayerPrefsController.IsSoundEffectsMute = muteFlags[2].ToString();
            PlayerPrefsController.HasPlayerPrefs = true;
        }
    }
}