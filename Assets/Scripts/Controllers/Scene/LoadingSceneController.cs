using Controllers.Core;
using Effects;
using MVC.Enums;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Scene
{
    /// <summary>
    /// Controller for Loading Scene
    /// </summary>
    public class LoadingSceneController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private GameObject[] instructionPanels;
        [SerializeField] private Button[] gotoButtons;
        [SerializeField] private Button continueButton;

        [Header("Labels to Translate Global")]
        [SerializeField] private TextMeshProUGUI instructionsLabel;

        [Header("Labels to Translate - Page 01")]
        [SerializeField] private TextMeshProUGUI touchpadLabel;
        [SerializeField] private TextMeshProUGUI shootButtonLabel;
        [SerializeField] private TextMeshProUGUI cursorLabel;
        [SerializeField] private TextMeshProUGUI pauseButtonLabel;

        [Header("Labels to Translate - Page 02")]
        [SerializeField] private TextMeshProUGUI lightBlockLabel;
        [SerializeField] private TextMeshProUGUI normalBlockLabel;
        [SerializeField] private TextMeshProUGUI darkBlockLabel;
        [SerializeField] private TextMeshProUGUI silverBlocksLabel;
        [SerializeField] private TextMeshProUGUI goldBlocksLabel;
        [SerializeField] private TextMeshProUGUI unbreakableBlockLabel;

        [Header("Labels to Translate - Page 03")]
        [SerializeField] private TextMeshProUGUI oneHitBlocksLabel;
        [SerializeField] private TextMeshProUGUI ballBiggerLabel;
        [SerializeField] private TextMeshProUGUI ballFasterLabel;
        [SerializeField] private TextMeshProUGUI resetBallsLabel;
        [SerializeField] private TextMeshProUGUI ballSlowerLabel;
        [SerializeField] private TextMeshProUGUI ballSmallerLabel;
        [SerializeField] private TextMeshProUGUI duplicateBallLabel;
        [SerializeField] private TextMeshProUGUI fireBallLabel;
        [SerializeField] private TextMeshProUGUI pullBlocksDownLabel;
        [SerializeField] private TextMeshProUGUI pullBlocksLeftLabel;
        [SerializeField] private TextMeshProUGUI pullBlocksRightLabel;
        [SerializeField] private TextMeshProUGUI pullBlocksUpLabel;
        [SerializeField] private TextMeshProUGUI expandPaddleLabel;
        [SerializeField] private TextMeshProUGUI resetPaddleLabel;
        [SerializeField] private TextMeshProUGUI shrinkPaddleLabel;
        [SerializeField] private TextMeshProUGUI shooterLabel;
        [SerializeField] private TextMeshProUGUI allBreakableBlocksLabel;

        // || Config

        private const float TIME_TO_WAIT = 1f;

        // || Cached

        private TextMeshProUGUI continueButtonLabel;

        private void Awake() => GetRequiredComponents();

        private void Start()
        {
            if (GameStatusController.Instance.NextSceneName.Equals(SceneManagerController.LevelSceneName))
            {
                Translate();
                BindEventListeners();
            }
            else
            {
                loadingPanel.SetActive(false);
                StartCoroutine(CallNextScene());
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                continueButtonLabel = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        private void Translate()
        {
            try
            {
                instructionsLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_instructions);
                touchpadLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_movepaddle);
                shootButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_releaseball);
                cursorLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_dragtoaim);
                pauseButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_pause);
                lightBlockLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_lightblock);
                normalBlockLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_normalblock);
                darkBlockLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_darkblock);
                silverBlocksLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_silverblock);
                goldBlocksLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_goldblock);
                unbreakableBlockLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.instructions_unbreakableblock);
                oneHitBlocksLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_onehitblock);
                ballBiggerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_ballbigger);
                ballFasterLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_ballfaster);
                resetBallsLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_resetballs);
                ballSlowerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_ballslower);
                ballSmallerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_ballsmaller);
                duplicateBallLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_duplicateball);
                fireBallLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_fireball);
                pullBlocksDownLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_pullblocksdown);
                pullBlocksLeftLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_pullblocksleft);
                pullBlocksRightLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_pullblocksright);
                pullBlocksUpLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_pullblocksup);
                expandPaddleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_expandpaddle);
                resetPaddleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_resetpaddle);
                shrinkPaddleLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_shrinkpaddle);
                shooterLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_shooter);
                allBreakableBlocksLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.powerups_allbreakablesblocks);
                continueButtonLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_continue);
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
                    canvasGroup.interactable = false;
                    StartCoroutine(CallNextScene());
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calls the next scene
        /// </summary>
        private IEnumerator CallNextScene()
        {
            yield return new WaitForSecondsRealtime(TIME_TO_WAIT);
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(FadeEffect.Instance.GetFadeOutLength());
            AsyncOperation operation = SceneManagerController.CallSceneAsync(GameStatusController.Instance.NextSceneName);
        }
    }
}