using Controllers.Core;
using Effects;
using MVC.BL;
using MVC.Enums;
using MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Panel
{
    /// <summary>
    /// Controller for Select Levels Panel
    /// </summary>
    public class SelectLevelsPanelController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button configurationsButton;
        [SerializeField] private Button soundtrackButton;
        [SerializeField] private Button quitApplicationButton;
        [SerializeField] private Sprite[] levelThumbnails;

        [Header("Levels Button")]
        [SerializeField] private ScrollRect levelButtonsScrollRect;
        [SerializeField] private GameObject levelButtonsContainer;
        [SerializeField] private LevelButton levelButtonPrefab;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private TextMeshProUGUI savingLabel;

        // || State

        [SerializeField] private int currentLevelIndex = 0;
        [SerializeField] private bool hasPlayerFinishedGame = false;
        private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;

        // || Config

        private const float TIME_TO_WAIT_AFTER_SAVE = 2f;

        // || Cached

        private PlayerProgress progress;
        private LevelBL levelBL;
        private ScoreboardBL scoreboardBL;
        private List<Level> levels;

        // || Properties

        public static SelectLevelsPanelController Instance { get; private set; }
        public Enumerators.GameStates ActualGameState { get => actualGameState; set => actualGameState = value; }

        private void Awake()
        {
            Instance = this;
            progress = new PlayerProgress();
            levelBL = new LevelBL();
            scoreboardBL = new ScoreboardBL();
            levels = new List<Level>();

            Translate();
            BindEventListeners();
        }

        private void Start()
        {
            AudioController.Instance.ChangeMusic(AudioController.Instance.SelectLevelsSong, false, string.Empty, true, false);
            GameStatusController.Instance.HasStartedSong = true;

            Time.timeScale = 1f;
            savingLabel.text = string.Empty;

            LoadProgress();
            CheckIfCameFromLevel();
            LoadLevels();
            UpdateUI();
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        private void Translate()
        {
            try
            {
                titleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.selectlevels_chooseanlevel);
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
                configurationsButton.onClick.AddListener(() =>
                {
                    TogglePanel(false);
                    ConfigurationPanelController.Instance.TogglePanel(true);
                });

                soundtrackButton.onClick.AddListener(() => StartCallNextScene(SceneManagerController.SceneNames.Soundtrack));
                quitApplicationButton.onClick.AddListener(() => SceneManagerController.QuitGame());
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
        /// Load current or new progress
        /// </summary>
        private void LoadProgress()
        {
            try
            {
                progress = ProgressManager.LoadProgress();
                currentLevelIndex = progress.CurrentLevelIndex;
                hasPlayerFinishedGame = progress.HasPlayerFinishedGame;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check if previous scene is a level
        /// </summary>
        private void CheckIfCameFromLevel()
        {
            try
            {
                if (!GameStatusController.Instance.CameFromLevel) return;

                currentLevelIndex = GameStatusController.Instance.LevelIndex;

                if (GameStatusController.Instance.IsLevelCompleted)
                {
                    Level current = levelBL.GetById(GameStatusController.Instance.LevelId);
                    Level next = levelBL.GetById(GameStatusController.Instance.LevelId + 1);
                    Level last = levelBL.GetLastLevel();

                    if (current != null && !current.IsCompleted)
                    {
                        levelBL.UpdateIsCompletedById(GameStatusController.Instance.LevelId, true);
                    }

                    Scoreboard scoreboard = new Scoreboard()
                    {
                        LevelId = GameStatusController.Instance.LevelId,
                        Score = GameStatusController.Instance.NewScore,
                        TimeScore = GameStatusController.Instance.NewTimeScore,
                        BestCombo = GameStatusController.Instance.NewCombo,
                        Moment = DateTimeOffset.Now.ToUnixTimeSeconds()
                    };

                    scoreboardBL.Insert(scoreboard);

                    if (next != null && !next.IsUnlocked && !next.IsCompleted)
                    {
                        levelBL.UpdateIsUnlockedById(next.Id, true);
                    }

                    if (!hasPlayerFinishedGame)
                    {
                        hasPlayerFinishedGame = last.IsCompleted;
                    }

                    StartCoroutine(SaveProgress());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Clear level items
        /// </summary>
        public IEnumerator ClearLevels()
        {
            foreach (Transform child in levelButtonsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitUntil(() => levelButtonsContainer.transform.childCount == 0);

            LoadLevels();
            yield return SaveProgress();
        }

        /// <summary>
        /// Load list of levels
        /// </summary>
        private void LoadLevels()
        {
            try
            {
                levels = levelBL.ListAll();
                for (int index = 0; index < levels.Count; index++)
                {
                    int currentIndex = index;
                    Level level = levels[currentIndex];
                    LevelButton levelButton = Instantiate(levelButtonPrefab);
                    levelButton.name = currentIndex.ToString("000");
                    levelButton.transform.SetParent(levelButtonsContainer.transform, false);
                    levelButton.transform.SetAsLastSibling();
                    levelButton.Set(currentIndex, level, OpenLevelDetails);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Open level details
        /// </summary>
        /// <param name="level"> Instance of Level </param>
        /// <param name="currentIndex"> Current index of Level </param>
        private void OpenLevelDetails(Level level, int currentIndex)
        {
            try
            {
                Scoreboard scoreboard = scoreboardBL.GetByMaxScoreByLevel(level.Id);
                string bestScore = Formatter.FormatToCurrency(scoreboard.Score);
                string bestTimeScore = (scoreboard.TimeScore == 0 ? string.Empty : Formatter.GetEllapsedTimeInHours(scoreboard.TimeScore));
                string bestCombo = (scoreboard.BestCombo == 0 ? string.Empty : scoreboard.BestCombo.ToString());
                LevelDetailsPanelController.Instance.UpdateUI(currentIndex + 1, level.Id, bestScore, bestTimeScore, bestCombo, levelThumbnails[currentIndex]);

                GameStatusController.Instance.LevelId = level.Id;
                GameStatusController.Instance.LevelIndex = currentIndex;
                GameStatusController.Instance.NewScore = 0;
                GameStatusController.Instance.NewTimeScore = 0;
                GameStatusController.Instance.NewCombo = 0;
                GameStatusController.Instance.OldScore = scoreboard.Score;
                GameStatusController.Instance.OldTimeScore = scoreboard.TimeScore;
                GameStatusController.Instance.OldCombo = scoreboard.BestCombo;

                TogglePanel(false);
                LevelDetailsPanelController.Instance.TogglePanel(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update UI elements
        /// </summary>
        private void UpdateUI()
        {
            try
            {
                int nextCurrentLevelIndex = (currentLevelIndex + 1);
                if (nextCurrentLevelIndex >= 4 && nextCurrentLevelIndex <= 99)
                {
                    nextCurrentLevelIndex = (!levels[nextCurrentLevelIndex].IsCompleted ? nextCurrentLevelIndex : currentLevelIndex);

                    Canvas.ForceUpdateCanvases();

                    GameObject button = GameObject.Find(nextCurrentLevelIndex.ToString("000"));
                    if (!button) return;

                    // Calculates new anchored position
                    RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
                    RectTransform containerRectTransform = levelButtonsContainer.GetComponent<RectTransform>();
                    Vector2 localContainerPosition = (Vector2)levelButtonsScrollRect.transform.InverseTransformPoint(containerRectTransform.position);
                    Vector2 localButtonPosition = (Vector2)levelButtonsScrollRect.transform.InverseTransformPoint(buttonRectTransform.position);
                    Vector2 newAnchoredPosition = (localContainerPosition - localButtonPosition);
                    float padding = 10;
                    float spacing = 100;
                    containerRectTransform.anchoredPosition = new Vector2(0, newAnchoredPosition.y - (padding + spacing));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// If the user wants to reset progress
        /// </summary>
        public void ResetProgress()
        {
            currentLevelIndex = 0;
            hasPlayerFinishedGame = false;
            scoreboardBL.DeleteAll();
            levelBL.ResetLevels();
            ProgressManager.DeleteProgress();
        }

        /// <summary>
        /// Call next scene
        /// </summary>
        public void StartCallNextScene(string sceneName) => StartCoroutine(CallNextScene(sceneName));

        /// <summary>
        /// Call next scene
        /// </summary>
        private IEnumerator CallNextScene(string sceneName)
        {
            ActualGameState = Enumerators.GameStates.TRANSITION;
            canvasGroup.interactable = false;

            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(FadeEffect.Instance.GetFadeOutLength());

            GameStatusController.Instance.NextSceneName = sceneName;
            GameStatusController.Instance.CameFromLevel = false;
            SceneManagerController.CallScene(SceneManagerController.SceneNames.Loading);
        }

        /// <summary>
        /// Save current progress
        /// </summary>
        private IEnumerator SaveProgress()
        {
            ActualGameState = Enumerators.GameStates.SAVE_LOAD;
            savingLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.selectlevels_saving);
            canvasGroup.interactable = false;

            progress.CurrentLevelIndex = currentLevelIndex;
            progress.HasPlayerFinishedGame = hasPlayerFinishedGame;

            ProgressManager.SaveProgress(progress);

            yield return new WaitForSecondsRealtime(TIME_TO_WAIT_AFTER_SAVE);
            savingLabel.text = string.Empty;
            canvasGroup.interactable = true;
            ActualGameState = Enumerators.GameStates.GAMEPLAY;
        }
    }
}