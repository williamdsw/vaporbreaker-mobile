using Controllers.Core;
using Core;
using Effects;
using MVC.Enums;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Menu
{
    public class PauseController : MonoBehaviour
    {
        // || Inspector References

        [Header("Pause UI Objects")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button pauseButtonMenu;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button levelsButton;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI headerLabel;

        // || State

        private bool pauseState = false;

        // || Cached

        private TextMeshProUGUI resumeButtonLabel;
        private TextMeshProUGUI restartButtonLabel;
        private TextMeshProUGUI levelsButtonLabel;

        // || Properties

        public static PauseController Instance { get; private set; }
        public bool CanPause { private get; set; } = true;

        private void Awake()
        {
            Instance = this;
            panel.SetActive(false);

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
                resumeButtonLabel = resumeButton.GetComponentInChildren<TextMeshProUGUI>();
                restartButtonLabel = restartButton.GetComponentInChildren<TextMeshProUGUI>();
                levelsButtonLabel = levelsButton.GetComponentInChildren<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        public void Translate()
        {
            try
            {
                headerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.pause_paused);
                resumeButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.pause_resume);
                restartButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.pause_restart);
                levelsButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.pause_levels);
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
                pauseButtonMenu.onClick.AddListener(() => PauseOrResumeGame());
                resumeButton.onClick.AddListener(() => PauseOrResumeGame());
                restartButton.onClick.AddListener(() => StartCoroutine(ResetLevelCoroutine()));
                levelsButton.onClick.AddListener(() => StartCoroutine(GotoNextScene(SceneManagerController.SelectLevelsSceneName)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pause or resume current game
        /// </summary>
        public void PauseOrResumeGame()
        {
            pauseState = !pauseState;
            panel.SetActive(pauseState);
            GameSessionController.Instance.ActualGameState = (pauseState ? Enumerators.GameStates.PAUSE : Enumerators.GameStates.GAMEPLAY);
        }

        /// <summary>
        /// Reset current level
        /// </summary>
        private IEnumerator ResetLevelCoroutine()
        {
            yield return FadeIn();
            PersistentData.Instance.DestroyInstance();
            GameSessionController.Instance.DestroyInstance();
            SceneManagerController.ReloadScene();
        }

        // Reset to Select Levels
        private IEnumerator GotoNextScene(string sceneName)
        {
            yield return FadeIn();
            GameSessionController.Instance.GotoScene(sceneName);
        }


        /// <summary>
        /// Apply fade in
        /// </summary>
        private IEnumerator FadeIn()
        {
            GameSessionController.Instance.ActualGameState = Enumerators.GameStates.TRANSITION;
            GameStatusController.Instance.IsLevelCompleted = false;
            GameStatusController.Instance.CameFromLevel = false;
            FadeEffect.Instance.ResetAnimationFunctions();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(FadeEffect.Instance.GetFadeOutLength());
        }
    }
}