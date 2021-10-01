﻿using Controllers.Core;
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
    public class LevelCompleteController : MonoBehaviour
    {
        [Header("Required Elements References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button continueButton;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI headerLabel;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI timeScoreLabel;
        [SerializeField] private TextMeshProUGUI bestComboLabel;
        [SerializeField] private TextMeshProUGUI numberOfBallsLabel;
        [SerializeField] private TextMeshProUGUI totalLabel;
        [SerializeField] private TextMeshProUGUI newScoreLabel;

        // || Config

        private const float DELAY_TIME = 1f;

        // || State

        private long currentScore = 0;
        private long timeScore = 0;
        private int bestCombo = 0;
        private int numberOfBalls = 0;
        private long totalScore = 0;

        // || Cached

        private TextMeshProUGUI continueButtonText;

        // || Properties

        public static LevelCompleteController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            panel.SetActive(false);
            GetRequiredComponents();
            BindEventListeners();
            TranslateLabels();
            continueButtonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                continueButtonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Translates the UI
        /// </summary>
        private void TranslateLabels()
        {
            try
            {
                headerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_levelcompleted);
                scoreLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_score);
                timeScoreLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.general_timescore);
                bestComboLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_bestcombo);
                numberOfBallsLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_currentballs);
                totalLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_total);
                newScoreLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_newscore);
                continueButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_continue);
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
                continueButton.onClick.AddListener(() =>
                {
                    continueButton.interactable = false;
                    StartCoroutine(FadeCoroutine());
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pass values
        /// </summary>
        /// <param name="timeScore"> Score for Time </param>
        /// <param name="bestCombo"> Best Combo </param>
        /// <param name="currentScore"> Current Score </param>
        public void CallLevelComplete(float timeScore, int bestCombo, long currentScore)
        {
            this.timeScore = Mathf.FloorToInt(timeScore);
            this.bestCombo = bestCombo;
            this.currentScore = currentScore;
            this.numberOfBalls = FindObjectsOfType<Ball>().Length;

            StartCoroutine(LevelComplete());
        }

        /// <summary>
        /// Calculates total score
        /// </summary>
        private void CalculateTotalScore()
        {
            long totalTimeScore = 1000000;
            long minutes = (timeScore - ((timeScore / 3600) * 3600)) / 60;
            totalTimeScore = (minutes != 0 ? totalTimeScore / minutes : totalTimeScore);

            totalScore = (currentScore + totalTimeScore);
            totalScore += (numberOfBalls >= 1 ? numberOfBalls * 10000 : 0);
            totalScore += (bestCombo > 1 ? bestCombo * 10000 : 0);

            // Update UI
            scoreLabel.text = string.Format("{0} : {1}", scoreLabel.text, Formatter.FormatToCurrency(currentScore));
            timeScoreLabel.text = string.Format("{0} : {1}", timeScoreLabel.text, (totalTimeScore > 0 ? Formatter.FormatToCurrency((long)totalTimeScore) : "0"));
            bestComboLabel.text = string.Format("{0} : {1}", bestComboLabel.text, bestCombo.ToString());
            numberOfBallsLabel.text = string.Format("{0} : {1}", numberOfBallsLabel.text, numberOfBalls.ToString());
            totalLabel.text = string.Format("{0} : {1}", totalLabel.text, Formatter.FormatToCurrency(totalScore));
        }

        /// <summary>
        /// Show information
        /// </summary>
        private IEnumerator LevelComplete()
        {
            foreach (Ball ball in FindObjectsOfType<Ball>())
            {
                Destroy(ball.gameObject);
            }

            foreach (PowerUp powerUp in FindObjectsOfType<PowerUp>())
            {
                Destroy(powerUp.gameObject);
            }

            AudioController.Instance.StopME();
            AudioController.Instance.StopMusic();
            CalculateTotalScore();

            // Values to be saved
            GameStatusController.Instance.NewScore = totalScore;
            GameStatusController.Instance.NewTimeScore = timeScore;
            GameStatusController.Instance.NewCombo = bestCombo;
            GameStatusController.Instance.IsLevelCompleted = true;

            // Plays success sound
            yield return new WaitForSecondsRealtime(DELAY_TIME);
            AudioController.Instance.PlaySFX(AudioController.Instance.SuccessEffect, AudioController.Instance.MaxSFXVolume);

            // Show panel
            yield return new WaitForSecondsRealtime(DELAY_TIME);
            panel.SetActive(true);

            // Shows each text
            yield return new WaitForSecondsRealtime(DELAY_TIME);
            yield return ShowElement(scoreLabel.gameObject.transform.parent.gameObject);
            yield return ShowElement(timeScoreLabel.gameObject.transform.parent.gameObject);
            yield return ShowElement(bestComboLabel.gameObject.transform.parent.gameObject);
            yield return ShowElement(numberOfBallsLabel.gameObject.transform.parent.gameObject);
            yield return ShowElement(totalLabel.gameObject.transform.parent.gameObject);

            // New score
            if (totalScore > GameStatusController.Instance.OldScore)
            {
                AudioController.Instance.PlaySFX(AudioController.Instance.NewScoreEffect, AudioController.Instance.MaxSFXVolume);
                newScoreLabel.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(DELAY_TIME * 2);
            }

            continueButton.gameObject.SetActive(true);
            continueButton.Select();
        }

        /// <summary>
        /// Show desired element
        /// </summary>
        /// <param name="element"> Element to be shown </param>
        private IEnumerator ShowElement(GameObject element)
        {
            AudioController.Instance.PlaySFX(AudioController.Instance.HittingFace, AudioController.Instance.MaxSFXVolume);
            element.SetActive(true);
            yield return new WaitForSecondsRealtime(DELAY_TIME / 2f);
        }

        /// <summary>
        /// Fade to Select Levels
        /// </summary>
        private IEnumerator FadeCoroutine()
        {
            FadeEffect.Instance.ResetAnimationFunctions();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(FadeEffect.Instance.GetFadeOutLength());
            GameSessionController.Instance.GotoScene(SceneManagerController.SelectLevelsSceneName);
        }
    }
}