using Core;
using Effects;
using MVC.Enums;
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
        [SerializeField] private TextMeshProUGUI headerLabel;

        // || State

        private bool pauseState = false;
        private List<TextMeshProUGUI> allPauseMenuButtonsTexts;

        // || Properties

        public bool CanPause { private get; set; } = true;

        public static PauseController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            pauseMenu.SetActive(false);
            allPauseMenuButtonsTexts = new List<TextMeshProUGUI>();
            foreach (Button item in allPauseMenuButtons)
            {
                allPauseMenuButtonsTexts.Add(item.GetComponentInChildren<TextMeshProUGUI>());
            }
        }

        private void Start()
        {
            TranslateLabels();
            BindButtonClickEvents();
        }

        private void TranslateLabels()
        {
            headerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.pause_paused);
            string[] words = 
            {
                LocalizationController.Instance.GetWord(LocalizationFields.pause_resume),
                LocalizationController.Instance.GetWord(LocalizationFields.pause_restart),
                LocalizationController.Instance.GetWord(LocalizationFields.pause_levels),
            };

            for (int index = 0; index < words.Length; index++)
            {
                allPauseMenuButtonsTexts[index].text = words[index];
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
            GameSessionController.Instance.ActualGameState = (pauseState ? Enumerators.GameStates.PAUSE : Enumerators.GameStates.GAMEPLAY);
        }

        // Reset actual level
        private IEnumerator ResetLevelCoroutine()
        {
            GameSessionController.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            FadeEffect.Instance.ResetAnimationFunctions();
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.IsLevelCompleted = false;
            GameStatusController.Instance.CameFromLevel = false;
            PersistentData.Instance.DestroyInstance();
            GameSessionController.Instance.DestroyInstance();
            SceneManagerController.ReloadScene();
        }

        // Reset to Select Levels
        private IEnumerator ResetGameCoroutine(string sceneName)
        {
            GameSessionController.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            FadeEffect.Instance.ResetAnimationFunctions();
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.IsLevelCompleted = false;
            GameStatusController.Instance.CameFromLevel = true;
            GameSessionController.Instance.GotoScene(sceneName);
        }
    }
}