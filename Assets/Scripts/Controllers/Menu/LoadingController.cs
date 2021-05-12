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
        [Header("UI Elements")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private GameObject[] instructionPanels;
        [SerializeField] private Button[] gotoButtons;
        [SerializeField] private Button continueButton;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // || Const

        private const float TIME_TO_WAIT = 1f;

        private void Start()
        {
            UnityUtilities.DisableAnalytics();

            if (GameStatusController.Instance.CameFromLevel ||
                GameStatusController.Instance.NextSceneName.Equals(SceneManagerController.SelectLevelsSceneName))
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

            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }
        }

        private void BindClickEvents()
        {
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
            // Fade Out effect
            yield return new WaitForSecondsRealtime(TIME_TO_WAIT);
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);

            // Calls next scene
            string nextSceneName = GameStatusController.Instance.NextSceneName;
            AsyncOperation operation = SceneManagerController.CallSceneAsync(nextSceneName);
        }
    }
}