using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button quitButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Header("Labels to Translate")]
    [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

    // State
    private bool isMute;
    [SerializeField] private bool[] muteFlags = { false, false, false };

    // Cached
    private AudioSource audioSourceBGM;
    private AudioSource audioSourceME;
    private AudioSource audioSourceSFX;

    private void Start()
    {
        if (!audioSourceSFX || !audioSourceME || !audioSourceSFX)
        {
            audioSourceBGM = GameObject.Find("SourceBGM").GetComponent<AudioSource>();
            audioSourceME = GameObject.Find("SourceME").GetComponent<AudioSource>();
            audioSourceSFX = GameObject.Find("SourceSFX").GetComponent<AudioSource>();
        }

        TranslateLabels();
        BindClickEvents();
        LoadSettings();
    }

    private void TranslateLabels()
    {
        // CANCELS
        if (!LocalizationController.Instance) return;

        List<string> labels = new List<string>();
        foreach (string label in LocalizationController.Instance.GetConfigurationsLabels())
        {
            labels.Add(label);
        }

        if (labels.Count == 0 || uiLabels.Count == 0) return;
        for (int index = 0; index < labels.Count; index++)
        {
            uiLabels[index].SetText(labels[index]);
        }
    }

    private void BindClickEvents()
    {
        if (volumeButtons.Length == 0 || muteFlags.Length == 0 || !resetProgressButton || !aboutButton || !quitButton) return;

        // BACKGROUND MUSIC
        volumeButtons[0].onClick.AddListener(() =>
        {
            Transform child = volumeButtons[0].transform.GetChild(0);
            Image image = child.GetComponent<Image>();
            ToggleAudio(audioSourceBGM, image, 0);
        });

        // MUSIC EFFECTS
        volumeButtons[1].onClick.AddListener(() =>
        {
            Transform child = volumeButtons[1].transform.GetChild(0);
            Image image = child.GetComponent<Image>();
            ToggleAudio(audioSourceME, image, 1);
        });

        // SOUND EFFECTS
        volumeButtons[2].onClick.AddListener(() =>
        {
            Transform child = volumeButtons[2].transform.GetChild(0);
            Image image = child.GetComponent<Image>();
            ToggleAudio(audioSourceSFX, image, 2);
        });

        aboutButton.onClick.AddListener(() =>
        {
            if (!configurationPanel || !aboutPanel) return;
            configurationPanel.SetActive(false);
            aboutPanel.SetActive(true);
        });

        resetProgressButton.onClick.AddListener(() =>
        {
            if (!confirmBox) return;
            configurationPanel.SetActive(false);
            confirmBox.SetActive(true);
        });

        quitButton.onClick.AddListener(() =>
        {
            if (!configurationPanel || !selectLevelsPanel) return;
            configurationPanel.SetActive(false);
            selectLevelsPanel.SetActive(true);
            SaveSettings();
        });
    }

    private void ToggleAudio(AudioSource source, Image image, int index)
    {
        if (!source || !image || !soundOnSprite || !soundOffSprite) return;

        isMute = !isMute;
        muteFlags[index] = isMute;
        source.mute = isMute;
        image.sprite = (isMute ? soundOffSprite : soundOnSprite);
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
        audioSourceBGM.mute = muteFlags[0];
        audioSourceME.mute = muteFlags[1];
        audioSourceSFX.mute = muteFlags[2];
    }

    private void UpdateUI()
    {
        if (volumeButtons.Length == 0) return;

        int index = 0;
        foreach (Button button in volumeButtons)
        {
            Transform child = button.transform.GetChild(0);
            Image image = child.GetComponent<Image>();
            image.sprite = (muteFlags[index] ? soundOffSprite : soundOnSprite);
            index++;
        }
    }

    private void LoadSettings()
    {
        if (!AudioController.Instance) return;

        if (!PlayerPrefsController.HasPlayerPrefs())
        {
            DefaultValues();
            return;
        }

        muteFlags[0] = bool.Parse(PlayerPrefsController.GetBGMVolumeMute());
        muteFlags[1] = bool.Parse(PlayerPrefsController.GetMEVolumeMute());
        muteFlags[2] = bool.Parse(PlayerPrefsController.GetSFXVolumeMute());
        UpdateUI();
        ApplyValues();
    }

    private void SaveSettings()
    {
        PlayerPrefsController.SetBGMVolumeMute(muteFlags[0].ToString());
        PlayerPrefsController.SetMEVolumeMute(muteFlags[1].ToString());
        PlayerPrefsController.SetSFXVolumeMute(muteFlags[2].ToString());
        PlayerPrefsController.SetHasPlayerPrefs(true.ToString());
    }
}