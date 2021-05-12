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

    public class LoadingController : MonoBehaviour
    {
        // Config
        [Header("UI Elements")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private GameObject[] instructionPanels;
        [SerializeField] private Button[] gotoButtons;
        [SerializeField] private Button continueButton;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        private float timeToWait = 1f;

        // State
        private AsyncOperation operation;

        // Cached
        private FadeEffect fadeEffect;

        private void Start()
        {
            UnityUtilities.DisableAnalytics();

            fadeEffect = FindObjectOfType<FadeEffect>();

            if (GameStatusController.Instance.GetCameFromLevel() ||
                GameStatusController.Instance.GetNextSceneName().Equals(SceneManagerController.SelectLevelsSceneName))
            {
                loadingPanel.SetActive(false);
                StartCoroutine(CallNextScene());
            }
            else
            {
                TranslateLabels();
                BindClickEvents();
            }
        }

        private void TranslateLabels()
        {
            // CANCELS
            if (!LocalizationController.Instance) return;

            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetInstructions01Labels())
            {
                labels.Add(label);
            }

            foreach (string label in LocalizationController.Instance.GetInstructions02Labels())
            {
                labels.Add(label);
            }

            foreach (string label in LocalizationController.Instance.GetInstructions03Labels())
            {
                labels.Add(label);
            }

            if (labels.Count == 0 || uiLabels.Count == 0 || labels.Count != uiLabels.Count) return;
            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindClickEvents()
        {
            // Check and Cancels
            if (instructionPanels.Length == 0 || gotoButtons.Length == 0 || !continueButton) return;

            // INSTRUCTION PANEL 01 - RIGHT BUTTON
            gotoButtons[0].onClick.AddListener(() =>
            {
                instructionPanels[0].SetActive(false);
                instructionPanels[1].SetActive(true);
            });

            // INSTRUCTION PANEL 02 - LEFT BUTTON
            gotoButtons[1].onClick.AddListener(() =>
            {
                instructionPanels[1].SetActive(false);
                instructionPanels[0].SetActive(true);
            });

            // INSTRUCTION PANEL 03 - RIGHT BUTTON
            gotoButtons[2].onClick.AddListener(() =>
            {
                instructionPanels[1].SetActive(false);
                instructionPanels[2].SetActive(true);
            });

            // INSTRUCTION PANEL 04 - LEFT BUTTON
            gotoButtons[3].onClick.AddListener(() =>
            {
                instructionPanels[2].SetActive(false);
                instructionPanels[1].SetActive(true);
            });

            continueButton.onClick.AddListener(() =>
            {
                continueButton.interactable = false;
                StartCoroutine(CallNextScene());
            });
        }

        private IEnumerator CallNextScene()
        {
            if (!fadeEffect || !GameStatusController.Instance) { yield return null; }

            // Fade Out effect
            yield return new WaitForSecondsRealtime(timeToWait);
            float fadeOutLength = fadeEffect.GetFadeOutLength();
            fadeEffect.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);

            // Calls next scene
            string nextSceneName = GameStatusController.Instance.GetNextSceneName();
            operation = SceneManagerController.CallSceneAsync(nextSceneName);
        }
    }
}