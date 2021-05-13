using Controllers.Core;
using Effects;
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
        [SerializeField] private List<string> levelsNamesList;
        [SerializeField] private List<bool> isLevelUnlockedList;
        [SerializeField] private List<bool> isLevelCompletedList;
        [SerializeField] private List<int> highScoresList;
        [SerializeField] private List<int> highTimeScoresList;
        [SerializeField] private Sprite[] levelThumbnails;
        private int defaultTime = 0;
        private int defaultScore = 0;
        private float timeToWaitAfterSave = 2f;
        private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;

        // Cached Others
        private PlayerProgress progress = new PlayerProgress();

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

        private void Awake() => Instance = this;

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
            LoadLevelThumbnails();
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
            for (int index = 0; index < progress.TotalNumberOfLevels; index++)
            {
                // GameObject
                GameObject levelButton = Instantiate(levelButtonPrefab) as GameObject;
                levelButton.transform.SetParent(levelButtonsContainer.transform, false);
                string levelButtonName = levelButton.name;
                levelButton.name = string.Concat("Level", "_", index.ToString("00"));

                // Effect
                if (isLevelUnlockedList[index])
                {
                    // TV Animation
                    GameObject levelNumberObject = levelButton.transform.GetChild(2).gameObject;
                    if (levelNumberObject)
                    {
                        TextMeshProUGUI levelNumberText = levelNumberObject.GetComponent<TextMeshProUGUI>();
                        levelNumberText.text = (index + 1).ToString("00");
                        levelNumberObject.SetActive(true);
                    }
                }
                else
                {
                    // Static Effect
                    GameObject staticEffect = levelButton.transform.GetChild(1).gameObject;
                    if (staticEffect != null) { staticEffect.SetActive(true); }
                }

                // Button
                Button button = levelButton.GetComponent<Button>();
                button.interactable = isLevelUnlockedList[index];
                button.onClick.AddListener(() =>
                {
                    // Data
                    string indexString = levelButton.name;
                    indexString = indexString.Replace("Level_", "");
                    int currentIndex = int.Parse(indexString);
                    string levelName = Formatter.FormatLevelName(levelsNamesList[currentIndex]);
                    levelName = levelName.Replace("Level ", "");
                    string bestScore = Formatter.FormatToCurrency(highScoresList[currentIndex]);
                    string bestTimeScore = (highTimeScoresList[currentIndex] == 0 ? string.Empty : Formatter.FormatEllapsedTime(highTimeScoresList[currentIndex]));
                    LevelDetailsController.Instance.LevelSceneName = levelsNamesList[currentIndex];
                    LevelDetailsController.Instance.UpdateUI(levelName, bestScore, bestTimeScore, levelThumbnails[currentIndex]);

                    // Pass data
                    GameStatusController.Instance.LevelIndex = currentIndex;
                    GameStatusController.Instance.NewScore = 0;
                    GameStatusController.Instance.NewTimeScore = 0;
                    GameStatusController.Instance.OldScore = highScoresList[currentIndex];
                    GameStatusController.Instance.OldTimeScore = highTimeScoresList[currentIndex];

                    // Panels
                    selectLevelsPanel.SetActive(false);
                    levelDetailsPanel.SetActive(true);
                });
            }
        }

        private void LoadLevelThumbnails()
        {
            string path = string.Concat(FileManager.FilesFolderPath, FileManager.LevelsThumbnailsPath);
            levelThumbnails = Resources.LoadAll<Sprite>(path);
        }

        private void LoadProgress()
        {
            progress = ProgressManager.LoadProgress();

            // Getting values
            currentLevelIndex = progress.CurrentLevelIndex;
            hasPlayerFinishedGame = progress.HasPlayerFinishedGame;
            levelsNamesList = progress.LevelNamesList;
            isLevelUnlockedList = progress.IsLevelUnlockedList;
            isLevelCompletedList = progress.IsLevelCompletedList;
            highScoresList = progress.HighScoresList;
            highTimeScoresList = progress.HighTimeScoresList;
        }

        private void VerifyIfCameFromLevel()
        {
            if (!GameStatusController.Instance.CameFromLevel) return;

            currentLevelIndex = GameStatusController.Instance.LevelIndex;

            // Status
            if (GameStatusController.Instance.IsLevelCompleted)
            {
                if (GameStatusController.Instance.NewScore > GameStatusController.Instance.OldScore)
                {
                    highScoresList[currentLevelIndex] = GameStatusController.Instance.NewScore;
                }
                else
                {
                    highScoresList[currentLevelIndex] = GameStatusController.Instance.OldScore;
                }

                if (GameStatusController.Instance.OldTimeScore == defaultTime)
                {
                    highTimeScoresList[currentLevelIndex] = GameStatusController.Instance.NewTimeScore;
                }
                else
                {
                    if (GameStatusController.Instance.NewTimeScore < GameStatusController.Instance.OldTimeScore)
                    {
                        highTimeScoresList[currentLevelIndex] = GameStatusController.Instance.NewTimeScore;
                    }
                    else
                    {
                        highTimeScoresList[currentLevelIndex] = GameStatusController.Instance.OldTimeScore;
                    }
                }

                if (!isLevelCompletedList[currentLevelIndex])
                {
                    isLevelCompletedList[currentLevelIndex] = true;
                }

                // Enable next stage
                if ((currentLevelIndex + 1) < levelsNamesList.Count)
                {
                    if (!isLevelCompletedList[currentLevelIndex + 1])
                    {
                        isLevelUnlockedList[currentLevelIndex + 1] = true;
                    }
                }

                // Checks if has finished the game
                if (!hasPlayerFinishedGame)
                {
                    int lastIndex = (progress.TotalNumberOfLevels - 1);
                    hasPlayerFinishedGame = isLevelCompletedList[lastIndex];
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
                nextCurrentLevelIndex = (!isLevelCompletedList[nextCurrentLevelIndex] ? nextCurrentLevelIndex : currentLevelIndex);

                // Needed to update the elements positions
                Canvas.ForceUpdateCanvases();

                // Finds next button and checks
                string nextButtonName = string.Concat("Level", "_", nextCurrentLevelIndex.ToString("00"));
                GameObject button = GameObject.Find(nextButtonName);
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

        public void ResetProgress()
        {
            // Clear all
            isLevelUnlockedList.Clear();
            isLevelCompletedList.Clear();
            highScoresList.Clear();
            highTimeScoresList.Clear();

            // Default values
            for (int index = 0; index < progress.TotalNumberOfLevels; index++)
            {
                isLevelUnlockedList.Add((index == 0 ? true : false));
                isLevelCompletedList.Add(false);
                highScoresList.Add(0);
                highTimeScoresList.Add(0);
            }
        }

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
            progress.IsLevelUnlockedList = isLevelUnlockedList;
            progress.IsLevelCompletedList = isLevelCompletedList;
            progress.HighScoresList = highScoresList;
            progress.HighTimeScoresList = highTimeScoresList;

            // Saves
            ProgressManager.SaveProgress(progress);

            // Waits and return
            yield return new WaitForSecondsRealtime(timeToWaitAfterSave);
            savingElement.SetActive(false);
            ActualGameState = Enumerators.GameStates.GAMEPLAY;
        }
    }
}