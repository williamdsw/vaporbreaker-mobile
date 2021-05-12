using Controllers.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Menu
{

    public class LevelDetailsController : MonoBehaviour
    {
        // Config
        [Header("UI Elements")]
        [SerializeField] private GameObject selectLevelsPanel;
        [SerializeField] private GameObject levelDetailsPanel;
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        [Header("Level Details Elements")]
        [SerializeField] private Image levelThumbnail;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI bestTimeScoreText;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // State
        private string levelSceneName;
        private string levelTranslated;

        // Cached
        private static LevelDetailsController instance;

        public void SetLevelSceneName(string levelSceneName)
        {
            this.levelSceneName = levelSceneName;
        }

        public static LevelDetailsController Instance { get => instance; }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            if (!LocalizationController.Instance) return;

            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetLevelDetailsLabels())
            {
                labels.Add(label);
            }

            if (labels.Count == 0 || uiLabels.Count == 0) return;
            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }

            levelTranslated = labels[0];
        }

        private void BindClickEvents()
        {
            if (!playButton || !quitButton) return;

            playButton.onClick.AddListener(() =>
            {
                if (!SelectLevelsController.Instance) return;
                SelectLevelsController.Instance.StartCallNextScene(levelSceneName);
            });

            quitButton.onClick.AddListener(() =>
            {
                if (!selectLevelsPanel || !levelDetailsPanel) return;
                levelDetailsPanel.SetActive(false);
                selectLevelsPanel.SetActive(true);
            });
        }

        public void UpdateUI(string levelName, string bestScore, string bestTimeScore, Sprite levelThumbnailSprite)
        {
            if (!levelNameText || !bestScoreText || !bestTimeScoreText || !levelThumbnail) return;

            levelNameText.SetText(string.Concat(levelTranslated, levelName));
            bestScoreText.SetText(bestScore);
            bestTimeScoreText.SetText(bestTimeScore);
            levelThumbnail.sprite = levelThumbnailSprite;
        }
    }
}