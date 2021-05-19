using Controllers.Core;
using Effects;
using MVC.Enums;
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

        // || Const

        private const float TIME_TO_WAIT = 1f;

        // || Cached

        private TextMeshProUGUI continueButtonLabel;

        private void Awake() => continueButtonLabel = continueButton.GetComponentInChildren<TextMeshProUGUI>();

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