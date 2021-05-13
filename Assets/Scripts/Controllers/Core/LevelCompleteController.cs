using Core;
using Effects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Core
{
    public class LevelCompleteController : MonoBehaviour
    {
        [Header("UI Objects")]
        [SerializeField] private GameObject levelCompletedPanel;
        [SerializeField] private List<TextMeshProUGUI> labelsText;
        [SerializeField] private TextMeshProUGUI newScoreText;
        [SerializeField] private Button continueButton;

        [Header("Labels to Translate")]
        [SerializeField] private List<TextMeshProUGUI> uiLabels = new List<TextMeshProUGUI>();

        // Config
        [SerializeField] private float defaultSecondsValue = 1f;

        // Data / State
        private int currentScore = 0;
        private int timeScore = 0;
        private int bestCombo = 0;
        private int numberOfBalls = 0;
        private int totalScore = 0;
        private float countdownTimer = 3f;

        private Ball[] balls;
        private List<string> cachedTexts = new List<string>();

        private void Start()
        {
            levelCompletedPanel.SetActive(false);
            TranslateLabels();
            DefaultUIValues();
            BindClickEvents();
        }

        // Translate labels based on choosed language
        private void TranslateLabels()
        {
            List<string> labels = new List<string>();
            foreach (string label in LocalizationController.Instance.GetLevelCompleteLabels())
            {
                labels.Add(label);
            }

            for (int index = 0; index < labels.Count; index++)
            {
                uiLabels[index].SetText(labels[index]);
            }

            cachedTexts = labels;
        }

        private void DefaultUIValues()
        {
            foreach (TextMeshProUGUI labelText in labelsText)
            {
                GameObject parent = labelText.gameObject.transform.parent.gameObject;
                parent.SetActive(false);
            }

            newScoreText.enabled = false;
            continueButton.gameObject.SetActive(false);
        }

        public void BindClickEvents()
        {
            continueButton.onClick.AddListener(() =>
            {
                continueButton.interactable = false;
                StartCoroutine(FadeCoroutine());
            });
        }

        public void CallLevelComplete(float timeScore, int bestCombo, int currentScore)
        {
            // Finds the balls
            balls = FindObjectsOfType<Ball>();

            // Passing values
            this.timeScore = Mathf.FloorToInt(timeScore);
            this.bestCombo = bestCombo;
            this.currentScore = currentScore;
            this.numberOfBalls = balls.Length;

            StartCoroutine(LevelComplete());
        }

        private void CalculateTotalScore()
        {
            // Calculates total time
            int totalTimeScore = 0;

            if (timeScore > 120) { totalTimeScore = 0; }
            else if (timeScore > 110 && timeScore <= 120) { totalTimeScore = 20000; }
            else if (timeScore > 100 && timeScore <= 110) { totalTimeScore = 40000; }
            else if (timeScore > 90 && timeScore <= 100) { totalTimeScore = 50000; }
            else if (timeScore > 80 && timeScore <= 90) { totalTimeScore = 60000; }
            else if (timeScore > 70 && timeScore <= 80) { totalTimeScore = 80000; }
            else if (timeScore > 60 && timeScore <= 70) { totalTimeScore = 100000; }
            else if (timeScore > 50 && timeScore <= 60) { totalTimeScore = 120000; }
            else if (timeScore > 40 && timeScore <= 50) { totalTimeScore = 130000; }
            else if (timeScore > 40 && timeScore <= 50) { totalTimeScore = 140000; }
            else if (timeScore > 30 && timeScore <= 40) { totalTimeScore = 150000; }
            else if (timeScore > 20 && timeScore <= 30) { totalTimeScore = 160000; }
            else if (timeScore > 10 && timeScore <= 20) { totalTimeScore = 180000; }
            else if (timeScore > 0 && timeScore <= 10) { totalTimeScore = 200000; }

            // Calculates
            totalScore = (currentScore + totalTimeScore);
            totalScore += (numberOfBalls >= 1 ? numberOfBalls * 50000 : 0);
            totalScore += (bestCombo > 1 ? bestCombo * 50000 : 0);

            // Update UI
            int[] values = { currentScore, totalTimeScore, bestCombo, numberOfBalls, totalScore };
            for (int index = 0; index < values.Length; index++)
            {
                TextMeshProUGUI label = labelsText[index];
                label.SetText(string.Concat(cachedTexts[index], values[index]));
            }
        }

        private IEnumerator LevelComplete()
        {
            // Destroy current objects
            foreach (Ball ball in balls)
            {
                ball.StopBall();
            }

            PowerUp[] powerUps = FindObjectsOfType<PowerUp>();
            foreach (PowerUp powerUp in powerUps)
            {
                powerUp.StopPowerUp();
            }

            AudioController.Instance.StopME();
            AudioController.Instance.StopMusic();
            CalculateTotalScore();

            // Pass values
            GameStatusController.Instance.NewScore = totalScore;
            GameStatusController.Instance.NewTimeScore = timeScore;
            GameStatusController.Instance.IsLevelCompleted = true;

            // Plays success sound
            yield return new WaitForSecondsRealtime(defaultSecondsValue);
            AudioController.Instance.PlaySFX(AudioController.Instance.SuccessEffect, AudioController.Instance.MaxSFXVolume);

            // Show panel
            yield return new WaitForSecondsRealtime(defaultSecondsValue);
            levelCompletedPanel.SetActive(true);

            // Shows each text
            yield return new WaitForSecondsRealtime(defaultSecondsValue);
            for (int index = 0; index < labelsText.Count; index++)
            {
                AudioController.Instance.PlaySFX(AudioController.Instance.HittingFace, AudioController.Instance.MaxSFXVolume);
                GameObject labelParent = labelsText[index].gameObject.transform.parent.gameObject;
                labelParent.SetActive(true);
                yield return new WaitForSecondsRealtime(defaultSecondsValue / 2);
            }

            // Case have a new score
            if (totalScore > GameStatusController.Instance.OldScore)
            {
                AudioController.Instance.PlaySFX(AudioController.Instance.NewScoreEffect, AudioController.Instance.MaxSFXVolume);
                newScoreText.enabled = true;
                newScoreText.GetComponent<FlashTextEffect>().enabled = true;
                yield return new WaitForSecondsRealtime(defaultSecondsValue * 2);
            }

            continueButton.gameObject.SetActive(true);
        }

        private IEnumerator FadeCoroutine()
        {
            FadeEffect.Instance.ResetAnimationFunctions();
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameSession.Instance.ResetGame(SceneManagerController.SelectLevelsSceneName);
        }
    }
}