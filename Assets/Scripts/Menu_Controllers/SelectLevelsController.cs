using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelsController : MonoBehaviour
{
    [Header ("Main Elements")]
    [SerializeField] private GameObject selectLevelsPanel;
    [SerializeField] private GameObject configurationPanel;
    [SerializeField] private GameObject levelDetailsPanel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header ("Select Levels Panel Elements")]
    [SerializeField] private GameObject savingElement;
    [SerializeField] private Button configurationsButton;
    [SerializeField] private Button quitApplicationButton;

    [Header ("Levels Button")]
    [SerializeField] private ScrollRect levelButtonsScrollRect;
    [SerializeField] private GameObject levelButtonsContainer;
    [SerializeField] private GameObject levelButtonPrefab;

    [Header ("Labels to Translate")]
    [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI> ();

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
    private FadeEffect fadeEffect;
    private PlayerProgress progress = new PlayerProgress ();
    private static SelectLevelsController instance;

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS

    public Enumerators.GameStates GetActualGameState () { return this.actualGameState; }
    public void SetActualGameState (Enumerators.GameStates newGameState) 
    { 
        this.actualGameState = newGameState;

        if (actualGameState == Enumerators.GameStates.GAMEPLAY)
        {
            canvasGroup.interactable = true;
        }
        else 
        {
            canvasGroup.interactable = false;
        }
    }

    //--------------------------------------------------------------------------------//
    // PROPERTIES

    public static SelectLevelsController Instance { get { return instance; }}

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Awake () 
    {
        instance = this;
    }

    private void Start () 
    {
        UnityUtilities.DisableAnalytics ();

        if (!AudioController.Instance || !GameStatusController.Instance) { return; }

        // Find 
        fadeEffect = FindObjectOfType<FadeEffect>();

        // Play music
        AudioController.Instance.ChangeMusic (AudioController.Instance.SelectLevelsSong, false, "", true, false);
        GameStatusController.Instance.SetHasStartedSong (true);

        // Resets for animation works
        Time.timeScale = 1f;
        savingElement.SetActive (false);

        LoadProgress ();
        VerifyIfCameFromLevel ();
        TranslateLabels ();
        BindClickEvents ();
        LoadLevelButtons ();
        LoadLevelThumbnails ();
        UpdateUI ();
    }

    //--------------------------------------------------------------------------------//
    // HELPER FUNCTIONS

    // Translate labels based on choosed language
    private void TranslateLabels ()
    {
        // CANCELS
        if (!LocalizationController.Instance) { return; }
        
        List<string> labels = new List<string> ();
        foreach (string label in LocalizationController.Instance.GetSelectLevelsLabels ()) { labels.Add (label); }
        if (labels.Count == 0 || uiLabels.Count == 0) { return; }
        for (int index = 0; index < labels.Count; index++) { uiLabels[index].SetText (labels[index]); }
    }

    private void BindClickEvents ()
    {
        // Cancels
        if (!configurationsButton || !quitApplicationButton) { return; }

        configurationsButton.onClick.AddListener (() =>
        {
            // Cancels
            if (actualGameState != Enumerators.GameStates.GAMEPLAY) { return; }
            if (!selectLevelsPanel || !configurationPanel) { return; }

            selectLevelsPanel.SetActive (false);
            configurationPanel.SetActive (true);
        });

        quitApplicationButton.onClick.AddListener (() => 
        {
            // Cancels
            if (actualGameState != Enumerators.GameStates.GAMEPLAY) { return; }
            SceneManagerController.QuitGame ();
        });
    }

    public void CleanLevelButtons ()
    {
        // Checks and cancels
        if (!levelButtonsContainer) { return; }

        for (int index = 0; index < levelButtonsContainer.transform.childCount; index++)
        {
            Transform child = levelButtonsContainer.transform.GetChild (index);
            Destroy (child.gameObject);
        }

        LoadLevelButtons ();
        StartCoroutine (SaveProgress ());
    }

    private void LoadLevelButtons ()
    {
        for (int index = 0; index < progress.GetTotalNumberOfLevels (); index++)
        {
            // GameObject
            GameObject levelButton = Instantiate (levelButtonPrefab) as GameObject;
            levelButton.transform.SetParent (levelButtonsContainer.transform, false);
            string levelButtonName = levelButton.name;
            levelButton.name = string.Concat ("Level", "_", index.ToString ("00"));

            // Effect
            if (isLevelUnlockedList[index])
            {
                // TV Animation
                GameObject levelNumberObject = levelButton.transform.GetChild (2).gameObject;
                if (levelNumberObject) 
                { 
                    TextMeshProUGUI levelNumberText = levelNumberObject.GetComponent<TextMeshProUGUI>();
                    levelNumberText.text = (index + 1).ToString ("00");
                    levelNumberObject.SetActive (true); 
                }
            }
            else 
            {
                // Static Effect
                GameObject staticEffect = levelButton.transform.GetChild (1).gameObject;
                if (staticEffect) { staticEffect.SetActive (true); }
            }

            // Button
            Button button = levelButton.GetComponent<Button>();
            button.interactable = isLevelUnlockedList[index];
            button.onClick.AddListener (() =>
            {
                // Cancels
                if (!LevelDetailsController.Instance || !GameStatusController.Instance) { return; }
                if (!selectLevelsPanel || !levelDetailsPanel) { return; }

                // Data
                string indexString = levelButton.name;
                indexString = indexString.Replace ("Level_", "");
                int currentIndex = int.Parse (indexString);
                string levelName = Formatter.FormatLevelName (levelsNamesList[currentIndex]);
                levelName = levelName.Replace ("Level ", "");
                string bestScore = Formatter.FormatToCurrency (highScoresList[currentIndex]);
                string bestTimeScore = (highTimeScoresList[currentIndex] == 0 ? string.Empty : Formatter.FormatEllapsedTime (highTimeScoresList[currentIndex]));
                LevelDetailsController.Instance.SetLevelSceneName (levelsNamesList[currentIndex]);
                LevelDetailsController.Instance.UpdateUI (levelName, bestScore, bestTimeScore, levelThumbnails[currentIndex]);

                // Pass data
                GameStatusController.Instance.SetLevelIndex (currentIndex);
                GameStatusController.Instance.SetNewScore (0);
                GameStatusController.Instance.SetNewTimeScore (0);
                GameStatusController.Instance.SetOldScore (highScoresList[currentIndex]);
                GameStatusController.Instance.SetOldTimeScore (highTimeScoresList[currentIndex]);

                // Panels
                selectLevelsPanel.SetActive (false);
                levelDetailsPanel.SetActive (true);
            });
        }
    }

    private void LoadLevelThumbnails ()
    {
        string path = string.Concat (FileManager.GetFilesFolderPath (), FileManager.GetLevelsThumbnailsPath ());
        levelThumbnails = Resources.LoadAll<Sprite>(path);
    }

    private void LoadProgress ()
    {
        progress = ProgressManager.LoadProgress ();

        // Getting values
        currentLevelIndex = progress.GetCurrentLevelIndex ();
        hasPlayerFinishedGame = progress.GetHasPlayerFinishedGame ();
        levelsNamesList = progress.GetLevelNamesList ();
        isLevelUnlockedList = progress.GetIsLevelUnlockedList ();
        isLevelCompletedList = progress.GetIsLevelCompletedList ();
        highScoresList = progress.GetHighScoresList ();
        highTimeScoresList = progress.GetHighTimeScoresList ();
    }

    private void VerifyIfCameFromLevel ()
    {
        // Cancels
        if (!GameStatusController.Instance) { return; }
        if (!GameStatusController.Instance.GetCameFromLevel ()) { return; }

        currentLevelIndex = GameStatusController.Instance.GetLevelIndex ();

        // Status
        if (GameStatusController.Instance.GetIsLevelCompleted ())
        {
            // Score
            if (GameStatusController.Instance.GetNewScore () > GameStatusController.Instance.GetOldScore ())
            {
                highScoresList[currentLevelIndex] = GameStatusController.Instance.GetNewScore ();    
            }
            else 
            {
                highScoresList[currentLevelIndex] = GameStatusController.Instance.GetOldScore ();  
            }

            // Old Score
            if (GameStatusController.Instance.GetOldTimeScore () == defaultTime)
            {
                highTimeScoresList[currentLevelIndex] = GameStatusController.Instance.GetNewTimeScore ();    
            }
            else 
            {
                if (GameStatusController.Instance.GetNewTimeScore () < GameStatusController.Instance.GetOldTimeScore ())
                {
                    highTimeScoresList[currentLevelIndex] = GameStatusController.Instance.GetNewTimeScore ();    
                }
                else 
                {
                    highTimeScoresList[currentLevelIndex] = GameStatusController.Instance.GetOldTimeScore ();  
                }
            }

            // Level Completed
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
                int lastIndex = (progress.GetTotalNumberOfLevels () - 1);
                hasPlayerFinishedGame = isLevelCompletedList[lastIndex]; 
            }

            StartCoroutine (SaveProgress ());
        }
    }

    public void StartCallNextScene (string nextSceneName)
    {
        if (actualGameState != Enumerators.GameStates.GAMEPLAY) { return; }
        StartCoroutine (CallNextScene (nextSceneName));
    }

    private void UpdateUI ()
    {
        // Check and Cancels
        if (!levelButtonsScrollRect || !levelButtonsContainer) { return; }

        int nextCurrentLevelIndex = (currentLevelIndex + 1);
        if (nextCurrentLevelIndex >= 4 && nextCurrentLevelIndex <= 99)
        {
            // Checks if next level was completed...
            nextCurrentLevelIndex = (!isLevelCompletedList[nextCurrentLevelIndex] ? nextCurrentLevelIndex : currentLevelIndex);

            // Needed to update the elements positions
            Canvas.ForceUpdateCanvases ();

            // Finds next button and checks
            string nextButtonName = string.Concat ("Level", "_", nextCurrentLevelIndex.ToString ("00"));
            GameObject button = GameObject.Find (nextButtonName);
            if (!button) { return; }

            // Calculates new anchored position
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            RectTransform containerRectTransform = levelButtonsContainer.GetComponent<RectTransform>();
            Vector2 localContainerPosition = (Vector2) levelButtonsScrollRect.transform.InverseTransformPoint (containerRectTransform.position);
            Vector2 localButtonPosition = (Vector2) levelButtonsScrollRect.transform.InverseTransformPoint (buttonRectTransform.position);
            Vector2 newAnchoredPosition = (localContainerPosition - localButtonPosition);
            float padding = 10;
            float spacing = 100;
            containerRectTransform.anchoredPosition = new Vector2 (0, newAnchoredPosition.y - (padding + spacing));
        }
    }

    public void ResetProgress ()
    {
        // Clear all
        isLevelUnlockedList.Clear ();
        isLevelCompletedList.Clear ();
        highScoresList.Clear ();
        highTimeScoresList.Clear ();

        // Default values
        for (int i = 0; i < progress.GetTotalNumberOfLevels (); i++)
        {
            isLevelUnlockedList.Add ((i == 0 ? true : false));
            isLevelCompletedList.Add (false);
            highScoresList.Add (0);
            highTimeScoresList.Add (0);
        }
    }

    //--------------------------------------------------------------------------------//
    // COROUTINES

    // Wait fade out length to fade out to next scene
    private IEnumerator CallNextScene (string nextSceneName)
    {
        // Cancels
        if (!fadeEffect || !GameStatusController.Instance) { yield return null; }

        this.SetActualGameState (Enumerators.GameStates.TRANSITION);

        // Fade Out effect
        float fadeOutLength = fadeEffect.GetFadeOutLength ();
        fadeEffect.FadeToLevel ();
        yield return new WaitForSecondsRealtime (fadeOutLength);

        // Pass data
        GameStatusController.Instance.SetNextSceneName (nextSceneName);
        GameStatusController.Instance.SetCameFromLevel (false);
        SceneManagerController.CallScene (SceneManagerController.GetLoadingSceneName ());
    }

    // Save progress
    private IEnumerator SaveProgress ()
    {
        this.SetActualGameState (Enumerators.GameStates.SAVE_LOAD);
        savingElement.SetActive (true);

        // Passing values
        progress.SetCurrentLevelIndex (currentLevelIndex);
        progress.SetHasPlayerFinishedGame (hasPlayerFinishedGame);
        progress.SetIsLevelUnlockedList (isLevelUnlockedList);
        progress.SetIsLevelCompletedList (isLevelCompletedList);
        progress.SetHighScoresList (highScoresList);
        progress.SetHighTimeScoresList (highTimeScoresList);

        // Saves
        ProgressManager.SaveProgress (progress);

        // Waits and return
        yield return new WaitForSecondsRealtime (timeToWaitAfterSave);
        savingElement.SetActive (false);
        this.SetActualGameState (Enumerators.GameStates.GAMEPLAY);
    }
}