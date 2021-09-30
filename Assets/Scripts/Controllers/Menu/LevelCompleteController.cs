using Core;
using Effects;
using MVC.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Core
{
    public class LevelCompleteController : MonoBehaviour
    {
        [Header("Required Elements References")]
        [SerializeField] private GameObject levelCompletedPanel;
        [SerializeField] private Button continueButton;

        [Header("Labels to Translate")]
        [SerializeField] private TextMeshProUGUI headerLabel;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeScoreText;
        [SerializeField] private TextMeshProUGUI bestComboText;
        [SerializeField] private TextMeshProUGUI numberOfBallsText;
        [SerializeField] private TextMeshProUGUI totalText;
        [SerializeField] private TextMeshProUGUI newScoreLabel;

        // Config
        [SerializeField] private float defaultSecondsValue = 1f;

        // Data / State
        private int currentScore = 0;
        private int timeScore = 0;
        private int bestCombo = 0;
        private int numberOfBalls = 0;
        private int totalScore = 0;
        private float countdownTimer = 3f;

        // || Cached

        private Ball[] balls;
        private List<string> cachedTexts = new List<string>();
        private TextMeshProUGUI continueButtonText;
        private GameObject[] parents;

        // || Properties

        public static LevelCompleteController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            continueButtonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            levelCompletedPanel.SetActive(false);
            TranslateLabels();
            DefaultUIValues();
            BindClickEvents();
        }

        private void TranslateLabels()
        {
            headerLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_levelcompleted);
            scoreText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_score);
            timeScoreText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_timescore);
            bestComboText.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_bestcombo);
            numberOfBallsText.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_currentballs);
            totalText.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_total);
            newScoreLabel.text = LocalizationController.Instance.GetWord(LocalizationFields.levelcomplete_newscore);
            continueButtonText.text = LocalizationController.Instance.GetWord(LocalizationFields.general_continue);
        }

        private void DefaultUIValues()
        {
            parents = new GameObject[]
            {
                scoreText.gameObject.transform.parent.gameObject,
                timeScoreText.gameObject.transform.parent.gameObject,
                bestComboText.gameObject.transform.parent.gameObject,
                numberOfBallsText.gameObject.transform.parent.gameObject,
                totalText.gameObject.transform.parent.gameObject,
            };

            foreach (GameObject parent in parents)
            {
                parent.SetActive(false);
            }

            newScoreLabel.enabled = false;
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
            scoreText.text = string.Concat(scoreText.text, " : ", currentScore);
            timeScoreText.text = string.Concat(timeScoreText.text, " : ", totalTimeScore);
            bestComboText.text = string.Concat(bestComboText.text, " : ", bestCombo);
            numberOfBallsText.text = string.Concat(numberOfBallsText.text, " : ", numberOfBalls);
            totalText.text = string.Concat(totalText.text, " : ", totalScore);
        }

        private IEnumerator LevelComplete()
        {
            foreach (Ball ball in balls)
            {
                Destroy(ball.gameObject);
            }

            PowerUp[] powerUps = FindObjectsOfType<PowerUp>();
            foreach (PowerUp powerUp in powerUps)
            {
                Destroy(powerUp.gameObject);
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
            foreach (GameObject parent in parents)
            {
                AudioController.Instance.PlaySFX(AudioController.Instance.HittingFace, AudioController.Instance.MaxSFXVolume);
                parent.SetActive(true);
                yield return new WaitForSecondsRealtime(defaultSecondsValue / 2f);
            }

            // Case have a new score
            if (totalScore > GameStatusController.Instance.OldScore)
            {
                AudioController.Instance.PlaySFX(AudioController.Instance.NewScoreEffect, AudioController.Instance.MaxSFXVolume);
                newScoreLabel.enabled = true;
                newScoreLabel.GetComponent<FlashTextEffect>().enabled = true;
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
            GameSessionController.Instance.GotoScene(SceneManagerController.SelectLevelsSceneName);
        }
    }
}