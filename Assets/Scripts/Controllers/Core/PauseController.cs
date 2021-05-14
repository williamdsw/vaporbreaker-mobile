using Core;
using Effects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Core
{
    public class PauseController : MonoBehaviour
    {
        // || Inspector References

        [Header("Pause UI Objects")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private Button pauseButtonMenu;
        [SerializeField] private Button[] allPauseMenuButtons;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // || State
        private bool pauseState = false;

        // || Properties

        public bool CanPause { private get; set; } = true;

        public static PauseController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            pauseMenu.SetActive(false);
        }

        private void Start()
        {
            TranslateLabels();
            BindButtonClickEvents();
        }

        // Translate labels based on choosed language
        private void TranslateLabels()
        {
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetPauseLabels())
            {
                labels.Add(label);
            }

            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindButtonClickEvents()
        {
            // 0 = Resume, 1 = Restart, 2 = Levels
            pauseButtonMenu.onClick.AddListener(() => PauseGame());
            allPauseMenuButtons[0].onClick.AddListener(() => PauseGame());
            allPauseMenuButtons[1].onClick.AddListener(() => StartCoroutine(ResetLevelCoroutine()));
            allPauseMenuButtons[2].onClick.AddListener(() => StartCoroutine(ResetGameCoroutine(SceneManagerController.SelectLevelsSceneName)));
        }

        public void PauseGame()
        {
            // State
            pauseState = !pauseState;
            pauseMenu.SetActive(pauseState);
            GameSession.Instance.ActualGameState = (pauseState ? Enumerators.GameStates.PAUSE : Enumerators.GameStates.GAMEPLAY);
        }

        // Reset actual level
        private IEnumerator ResetLevelCoroutine()
        {
            GameSession.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            FadeEffect.Instance.ResetAnimationFunctions();
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.IsLevelCompleted = false;
            GameStatusController.Instance.CameFromLevel = false;
            PersistentData.Instance.DestroyInstance();
            GameSession.Instance.DestroyInstance();
            SceneManagerController.ReloadScene();
        }

        // Reset to Select Levels
        private IEnumerator ResetGameCoroutine(string sceneName)
        {
            GameSession.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            FadeEffect.Instance.ResetAnimationFunctions();
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.IsLevelCompleted = false;
            GameStatusController.Instance.CameFromLevel = true;
            GameSession.Instance.ResetGame(sceneName);
        }
    }
}