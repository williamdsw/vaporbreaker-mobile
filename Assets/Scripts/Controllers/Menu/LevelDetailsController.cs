using Controllers.Core;
using MVC.Enums;
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
        [SerializeField] private Button backButton;
        [SerializeField] private Button scoreboardButton;

        [Header("Level Details Elements")]
        [SerializeField] private Image levelThumbnail;
        [SerializeField] private TextMeshProUGUI levelNameText;
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI bestTimeScoreText;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI levelNameLabel;
        [SerializeField] private TextMeshProUGUI bestScoreLabel;
        [SerializeField] private TextMeshProUGUI bestTimeLabel;

        // || State
        private string levelTranslated;
        private long levelId;

        // || Cached

        private TextMeshProUGUI playButtonLabel;

        // || Properties

        public static LevelDetailsController Instance { get; private set; }
        public string LevelSceneName { private get; set; }

        private void Awake() => Instance = this;

        private void Start()
        {
            scoreboardButton.gameObject.SetActive(false);
            playButtonLabel = playButton.GetComponentInChildren<TextMeshProUGUI>();
            TranslateLabels();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            levelTranslated = levelNameLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_level);
            bestScoreLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_bestscore);
            bestTimeLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_besttime);
            playButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.leveldetails_play);
        }

        private void BindClickEvents()
        {
            playButton.onClick.AddListener(() => SelectLevelsController.Instance.StartCallNextScene(SceneManagerController.Level));
            backButton.onClick.AddListener(() =>
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
            levelNameText.SetText(string.Concat(levelTranslated, " ", levelName));
            bestScoreText.SetText(bestScore);
            bestTimeScoreText.SetText(bestTimeScore);
            levelThumbnail.sprite = levelThumbnailSprite;
            scoreboardButton.gameObject.SetActive(!string.IsNullOrEmpty(bestScore) && !string.IsNullOrEmpty(bestTimeScore));
            this.levelId = levelId;
        }
    }
}