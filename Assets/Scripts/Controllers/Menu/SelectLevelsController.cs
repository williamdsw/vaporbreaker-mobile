using Controllers.Core;
using Effects;
using MVC.BL;
using MVC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Menu
{
    public class SelectLevelsController : MonoBehaviour
    {
        [Header("Main Elements")]
        [SerializeField] private GameObject selectLevelsPanel;
        [SerializeField] private GameObject configurationPanel;
        [SerializeField] private GameObject levelDetailsPanel;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Select Levels Panel Elements")]
        [SerializeField] private GameObject savingElement;
        [SerializeField] private Button configurationsButton;
        [SerializeField] private Button quitApplicationButton;

        [Header("Levels Button")]
        [SerializeField] private ScrollRect levelButtonsScrollRect;
        [SerializeField] private GameObject levelButtonsContainer;
        [SerializeField] private GameObject levelButtonPrefab;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // State
        [SerializeField] private int currentLevelIndex = 0;
        [SerializeField] private bool hasPlayerFinishedGame = false;
        [SerializeField] private Sprite[] levelThumbnails;
        private float timeToWaitAfterSave = 2f;
        private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;

        // || Cached

        private PlayerProgress progress = new PlayerProgress();
        private LevelBL levelBL;
        private ScoreboardBL scoreboardBL;
        private List<Level> levels;

        // || Properties

        public static SelectLevelsController Instance { get; private set; }
        public Enumerators.GameStates ActualGameState
        {
            get => actualGameState;
            set
            {
                actualGameState = value;
                canvasGroup.interactable = (ActualGameState == Enumerators.GameStates.GAMEPLAY);
            }
        }

        private void Awake()
        {
            Instance = this;
            levelBL = new LevelBL();
            scoreboardBL = new ScoreboardBL();
            levels = new List<Level>();
        }

        private void Start()
        {
            UnityUtilities.DisableAnalytics();

            // Play music
            AudioController.Instance.ChangeMusic(AudioController.Instance.SelectLevelsSong, false, "", true, false);
            GameStatusController.Instance.HasStartedSong = true;

            // Resets for animation works
            Time.timeScale = 1f;
            savingElement.SetActive(false);

            LoadProgress();
            VerifyIfCameFromLevel();
            TranslateLabels();
            BindClickEvents();
            LoadLevelButtons();
            UpdateUI();
        }

        private void TranslateLabels()
        {
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetSelectLevelsLabels())
            {
                labels.Add(label);
            }

            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindClickEvents()
        {
            configurationsButton.onClick.AddListener(() =>
            {
                if (ActualGameState != Enumerators.GameStates.GAMEPLAY) return;
                selectLevelsPanel.SetActive(false);
                configurationPanel.SetActive(true);
            });

            quitApplicationButton.onClick.AddListener(() =>
            {
                if (ActualGameState != Enumerators.GameStates.GAMEPLAY) return;
                SceneManagerController.QuitGame();
            });
        }

        public IEnumerator ClearLevelButtons()
        {
            foreach (Transform child in levelButtonsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            yield return new WaitUntil(() => levelButtonsContainer.transform.childCount == 0);

            LoadLevelButtons();
            StartCoroutine(SaveProgress());
        }

        private void LoadLevelButtons()
        {
            levels = levelBL.ListAll();
            for (int index = 0; index < levels.Count; index++)
            {
                Level level = levels[index];
                int currentIndex = index;

                // GameObject
                GameObject levelButton = Instantiate(levelButtonPrefab) as GameObject;
                levelButton.name = level.Name;
                levelButton.transform.SetParent(levelButtonsContainer.transform, false);
                levelButton.transform.SetAsLastSibling();

                // Effect
                if (level.IsUnlocked)
                {
                    // TV Animation
                    GameObject levelNumberObject = levelButton.transform.GetChild(2).gameObject;
                    if (levelNumberObject)
                    {
                        TextMeshProUGUI levelNumberText = levelNumberObject.GetComponent<TextMeshProUGUI>();
                        levelNumberText.text = (index + 1).ToString("000");
                        levelNumberObject.SetActive(true);
                    }
                }
                else
                {
                    // Static Effect
                    GameObject staticEffect = levelButton.transform.GetChild(1).gameObject;
                    if (staticEffect != null)
                    {
                        staticEffect.SetActive(true);
                    }
                }

                // Button
                Button button = levelButton.GetComponent<Button>();
                button.interactable = level.IsUnlocked;
                button.onClick.AddListener(() => OpenLevelDetails(level, currentIndex));
            }
        }
        private void OpenLevelDetails(Level level, int currentIndex)
        {
            Scoreboard scoreboard = scoreboardBL.GetByMaxScoreByLevel(level.Id);
            string levelName = Formatter.FormatLevelName(level.Name);
            string bestScore = Formatter.FormatToCurrency(scoreboard.Score);
            string bestTimeScore = (scoreboard.TimeScore == 0 ? string.Empty : Formatter.FormatEllapsedTime(scoreboard.TimeScore));
            LevelDetailsController.Instance.UpdateUI(levelName, bestScore, bestTimeScore, levelThumbnails[currentIndex], level.Id);
            LevelDetailsController.Instance.LevelSceneName = level.Name;

            // Pass data
            GameStatusController.Instance.LevelId = level.Id;
            GameStatusController.Instance.LevelIndex = currentIndex;
            GameStatusController.Instance.NewScore = 0;
            GameStatusController.Instance.NewTimeScore = 0;
            GameStatusController.Instance.OldScore = scoreboard.Score;
            GameStatusController.Instance.OldTimeScore = scoreboard.TimeScore;
            

            // Panels
            selectLevelsPanel.SetActive(false);
            levelDetailsPanel.SetActive(true);
        }

        private void LoadProgress()
        {
            progress = ProgressManager.LoadProgress();

            // Getting values
            currentLevelIndex = progress.CurrentLevelIndex;
            hasPlayerFinishedGame = progress.HasPlayerFinishedGame;
        }

        private void VerifyIfCameFromLevel()
        {
            if (!GameStatusController.Instance.CameFromLevel) return;

            currentLevelIndex = GameStatusController.Instance.LevelIndex;

            // Status
            if (GameStatusController.Instance.IsLevelCompleted)
            {
                Level current = levelBL.GetById(GameStatusController.Instance.LevelId);
                Level next = levelBL.GetById(GameStatusController.Instance.LevelId + 1);
                Level last = levelBL.GetLastLevel();

                if (current != null && !current.IsCompleted)
                {
                    levelBL.UpdateIsCompletedById(GameStatusController.Instance.LevelId, true);
                }

                Scoreboard scoreboard = new Scoreboard();
                scoreboard.LevelId = GameStatusController.Instance.LevelId;
                scoreboard.Score = GameStatusController.Instance.NewScore;
                scoreboard.TimeScore = GameStatusController.Instance.NewTimeScore;
                scoreboard.Moment = DateTimeOffset.Now.ToUnixTimeSeconds();
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

        public void StartCallNextScene(string nextSceneName)
        {
            if (ActualGameState != Enumerators.GameStates.GAMEPLAY) return;
            StartCoroutine(CallNextScene(nextSceneName));
        }

        private void UpdateUI()
        {
            int nextCurrentLevelIndex = (currentLevelIndex + 1);
            if (nextCurrentLevelIndex >= 4 && nextCurrentLevelIndex <= 99)
            {
                // Checks if next level was completed...
                nextCurrentLevelIndex = (!levels[nextCurrentLevelIndex].IsCompleted ? nextCurrentLevelIndex : currentLevelIndex);

                // Needed to update the elements positions
                Canvas.ForceUpdateCanvases();

                // Finds next button and checks
                GameObject button = GameObject.Find(levels[nextCurrentLevelIndex].Name);
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

        public void ResetProgress() => scoreboardBL.DeleteAll();

        // Wait fade out length to fade out to next scene
        private IEnumerator CallNextScene(string nextSceneName)
        {
            ActualGameState = Enumerators.GameStates.TRANSITION;

            // Fade Out effect
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);

            // Pass data
            GameStatusController.Instance.NextSceneName = nextSceneName;
            GameStatusController.Instance.CameFromLevel = false;
            SceneManagerController.CallScene(SceneManagerController.LoadingSceneName);
        }

        // Save progress
        private IEnumerator SaveProgress()
        {
            ActualGameState = Enumerators.GameStates.SAVE_LOAD;
            savingElement.SetActive(true);

            // Passing values
            progress.CurrentLevelIndex = currentLevelIndex;
            progress.HasPlayerFinishedGame = hasPlayerFinishedGame;

            // Saves
            ProgressManager.SaveProgress(progress);

            // Waits and return
            yield return new WaitForSecondsRealtime(timeToWaitAfterSave);
            savingElement.SetActive(false);
            ActualGameState = Enumerators.GameStates.GAMEPLAY;
        }
    }
}