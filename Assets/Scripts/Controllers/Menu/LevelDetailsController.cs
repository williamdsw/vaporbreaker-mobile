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
        [SerializeField] private Button scoreboardButton;

        [Header("Level Details Elements")]
        [SerializeField] private Image levelThumbnail;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI bestTimeScoreText;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // || State
        private string levelTranslated;
        private long levelId;

        // || Properties

        public static LevelDetailsController Instance { get; private set; }
        public string LevelSceneName { private get; set; }

        private void Awake() => Instance = this;

        private void Start()
        {
            scoreboardButton.gameObject.SetActive(false);
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetLevelDetailsLabels())
            {
                labels.Add(label);
            }

            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }

            levelTranslated = labels[0];
        }

        private void BindClickEvents()
        {
            playButton.onClick.AddListener(() => SelectLevelsController.Instance.StartCallNextScene(LevelSceneName));
            quitButton.onClick.AddListener(() =>
            {
                levelDetailsPanel.SetActive(false);
                selectLevelsPanel.SetActive(true);
            });
            scoreboardButton.onClick.AddListener(() =>
            {
                levelDetailsPanel.SetActive(false);
                ScoreboardController.Instance.RenderScoreboard(levelId);
            });
        }

        public void UpdateUI(string levelName, string bestScore, string bestTimeScore, Sprite levelThumbnailSprite, long levelId)
        {
            levelNameText.SetText(string.Concat(levelTranslated, levelName));
            bestScoreText.SetText(bestScore);
            bestTimeScoreText.SetText(bestTimeScore);
            levelThumbnail.sprite = levelThumbnailSprite;
            scoreboardButton.gameObject.SetActive(!string.IsNullOrEmpty(bestScore) && !string.IsNullOrEmpty(bestTimeScore));
            this.levelId = levelId;
        }
    }
}