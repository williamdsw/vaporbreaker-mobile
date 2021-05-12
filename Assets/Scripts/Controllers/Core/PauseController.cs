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
        [Header("Pause UI Objects")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private Button pauseButtonMenu;
        [SerializeField] private Button[] allPauseMenuButtons;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // State
        private bool canPause = true;
        private bool pauseState = false;

        // Cached
        private FadeEffect fadeEffect;

        public bool GetCanPause()
        {
            return canPause;
        }

        public void SetCanPause(bool canPause)
        {
            this.canPause = canPause;
        }

        private void Start()
        {
            fadeEffect = FindObjectOfType<FadeEffect>();

            if (pauseMenu)
            {
                pauseMenu.SetActive(false);
            }

            TranslateLabels();
            BindButtonClickEvents();
        }

        // Translate labels based on choosed language
        private void TranslateLabels()
        {
            if (!LocalizationController.Instance) return;
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetPauseLabels())
            {
                labels.Add(label);
            }

            if (labels.Count == 0 || uiLabels.Count == 0 || labels.Count != uiLabels.Count) return;
            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindButtonClickEvents()
        {
            if (!pauseButtonMenu || allPauseMenuButtons.Length == 0) return;

            // 0 = Resume, 1 = Restart, 2 = Levels
            pauseButtonMenu.onClick.AddListener(() => PauseGame());
            allPauseMenuButtons[0].onClick.AddListener(() => PauseGame());
            allPauseMenuButtons[1].onClick.AddListener(() => StartCoroutine(ResetLevelCoroutine()));
            allPauseMenuButtons[2].onClick.AddListener(() => StartCoroutine(ResetGameCoroutine(SceneManagerController.SelectLevelsSceneName)));
        }

        public void PauseGame()
        {
            if (!GameSession.Instance) return;

            // State
            pauseState = !pauseState;
            pauseMenu.SetActive(pauseState);
            GameSession.Instance.SetActualGameState(pauseState ? Enumerators.GameStates.PAUSE : Enumerators.GameStates.GAMEPLAY);
        }

        // Reset actual level
        private IEnumerator ResetLevelCoroutine()
        {
            if (!GameSession.Instance || !GameStatusController.Instance || !PersistentData.Instance) { yield return null; }

            GameSession.Instance.SetActualGameState(Enumerators.GameStates.TRANSITION);

            // Fades Out
            if (!fadeEffect)
            {
                fadeEffect = FindObjectOfType<FadeEffect>();
            }

            fadeEffect.ResetAnimationFunctions();
            float fadeOutLength = fadeEffect.GetFadeOutLength();
            fadeEffect.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.SetIsLevelCompleted(false);
            GameStatusController.Instance.SetCameFromLevel(false);
            PersistentData.Instance.DestroyInstance();
            GameSession.Instance.DestroyInstance();
            SceneManagerController.ReloadScene();
        }

        // Reset to Select Levels
        private IEnumerator ResetGameCoroutine(string sceneName)
        {
            if (!GameSession.Instance || !GameStatusController.Instance) { yield return null; }

            GameSession.Instance.SetActualGameState(Enumerators.GameStates.TRANSITION);

            // Fades Out
            if (!fadeEffect)
            {
                fadeEffect = FindObjectOfType<FadeEffect>();
            }

            fadeEffect.ResetAnimationFunctions();
            float fadeOutLength = fadeEffect.GetFadeOutLength();
            fadeEffect.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.SetIsLevelCompleted(false);
            GameStatusController.Instance.SetCameFromLevel(true);
            GameSession.Instance.ResetGame(sceneName);
        }
    }
}