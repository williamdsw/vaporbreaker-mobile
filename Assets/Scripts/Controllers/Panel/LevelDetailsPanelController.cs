using Controllers.Core;
using MVC.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Panel
{
    /// <summary>
    /// Controllers for Level Details
    /// </summary>
    public class LevelDetailsPanelController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button playButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button scoreboardButton;

        [Header("Level Details Elements")]
        [SerializeField] private Image levelThumbnail;
        [SerializeField] private TextMeshProUGUI bestScoreValueLabel;
        [SerializeField] private TextMeshProUGUI bestTimeScoreValueLabel;
        [SerializeField] private TextMeshProUGUI bestComboValueLabel;


        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI levelNameLabel;
        [SerializeField] private TextMeshProUGUI bestScoreLabel;
        [SerializeField] private TextMeshProUGUI bestTimeScoreLabel;
        [SerializeField] private TextMeshProUGUI bestComboLabel;

        // || State

        private long levelId;

        // || Cached

        private TextMeshProUGUI playButtonLabel;

        // || Properties

        public static LevelDetailsPanelController Instance { get; private set; }

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
                playButtonLabel = playButton.GetComponentInChildren<TextMeshProUGUI>();
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
                playButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_play);
                bestScoreLabel.SetText(LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_bestscore));
                bestTimeScoreLabel.SetText(LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_besttime));
                bestComboLabel.SetText(LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_bestcombo));
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
                playButton.onClick.AddListener(() => SelectLevelsPanelController.Instance.StartCallNextScene(SceneManagerController.SceneNames.Level));
                backButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    SelectLevelsPanelController.Instance.TogglePanel(true);
                });

                scoreboardButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    ScoreboardPanelController.Instance.Show(levelId);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update UI Elements
        /// </summary>
        /// <param name="levelIndex"> Index of Level </param>
        /// <param name="bestScore"> Max Best Score</param>
        /// <param name="bestTimeScore"> Max Best Time Score </param>
        /// <param name="bestCombo"> Best Combo </param>
        /// <param name="levelThumbnailSprite"> Level Thumbnail </param>
        /// <param name="levelId"> Level Id </param>
        public void UpdateUI(int levelIndex, long levelId, string bestScore, string bestTimeScore, string bestCombo, Sprite levelThumbnailSprite)
        {
            try
            {
                this.levelId = levelId;
                levelNameLabel.text = string.Format("{0} {1}", LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_level), levelIndex.ToString("000"));
                bestScoreValueLabel.SetText(bestScore);
                bestTimeScoreValueLabel.SetText(bestTimeScore);
                bestComboValueLabel.SetText(bestCombo);
                levelThumbnail.sprite = levelThumbnailSprite;
                scoreboardButton.gameObject.SetActive(!string.IsNullOrEmpty(bestScore) && !string.IsNullOrEmpty(bestTimeScore) && !string.IsNullOrEmpty(bestCombo));
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
    }
}