using Controllers.Core;
using Effects;
using MVC.Models;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Scene
{
    /// <summary>
    /// Controller for Soundtrack Scene
    /// </summary>
    public class SoundtrackSceneController : MonoBehaviour
    {
        // || Inspector References

        [Header("Music Player - Information")]
        [SerializeField] private TextMeshProUGUI trackNameLabel;
        [SerializeField] private TextMeshProUGUI artistNameLabel;

        [Header("Music Player - Timing")]
        [SerializeField] private Slider trackSlider;
        [SerializeField] private TextMeshProUGUI currentDurationLabel;
        [SerializeField] private TextMeshProUGUI totalDurationLabel;

        [Header("Music Player - Controls")]
        [SerializeField] private Button quitButton;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button repeatButton;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor;

        [Header("Required UI Elements - Others")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI hourLabel;
        [SerializeField] private TextMeshProUGUI batteryLevelLabel;

        // || State

        private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;
        private int currentSongIndex = 0;
        private float songEllapsedTime = 0f;
        private float songDuration = 0f;
        private bool isSongPaused = true;
        private bool isSongRepeated = false;
        private bool canEllapseTime = false;
        private bool isMuted = false;

        // || Cached

        private Image repeatButtonImage;
        private Image pauseButtonImage;

        private void Awake()
        {
            Time.timeScale = 1f;

            GetRequiredComponents();
            BindEventListeners();
            GetSongInfo();

            defaultColor = repeatButtonImage.color;
            AudioController.Instance.AudioSourceBGM.Stop();
            isMuted = AudioController.Instance.AudioSourceBGM.mute;
            AudioController.Instance.AudioSourceBGM.mute = false;
        }

        private void Update()
        {
            if (actualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (AudioController.Instance.IsSongPlaying && !isSongPaused && canEllapseTime)
                {
                    ShowEllapsedSongTime();
                }
            }

            UpdateHourText();
            UpdateBatteryLevel();
        }

        /// <summary>
        /// Get required components
        /// </summary>
        private void GetRequiredComponents()
        {
            try
            {
                repeatButtonImage = repeatButton.GetComponentInChildren<Image>();
                pauseButtonImage = pauseButton.GetComponentInChildren<Image>();
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
                quitButton.onClick.AddListener(() => StartCoroutine(CallNextScene()));

                previousButton.onClick.AddListener(() =>
                {
                    currentSongIndex--;
                    currentSongIndex = (currentSongIndex < 0 ? AudioController.Instance.AllNotLoopedSongs.Length - 1 : currentSongIndex);
                    PlaySong(currentSongIndex);
                });

                playButton.onClick.AddListener(() => PlaySong(currentSongIndex));

                pauseButton.onClick.AddListener(() =>
                {
                    if (AudioController.Instance.IsSongPlaying)
                    {
                        isSongPaused = !isSongPaused;
                        pauseButtonImage.color = (isSongPaused ? selectedColor : defaultColor);
                        AudioController.Instance.PauseMusic(isSongPaused);
                    }
                });

                nextButton.onClick.AddListener(() =>
                {
                    currentSongIndex++;
                    currentSongIndex = (currentSongIndex >= AudioController.Instance.AllNotLoopedSongs.Length ? 0 : currentSongIndex);
                    PlaySong(currentSongIndex);
                });

                repeatButton.onClick.AddListener(() =>
                {
                    isSongRepeated = !isSongRepeated;
                    AudioController.Instance.ToggleRepeatTrack(isSongRepeated);
                    repeatButtonImage.color = (isSongRepeated ? selectedColor : defaultColor);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Play the selected song
        /// </summary>
        /// <param name="index"> Song index at list </param>
        private void PlaySong(int index)
        {
            songEllapsedTime = 0;
            canEllapseTime = true;
            isSongPaused = false;
            AudioController.Instance.StopMusic();
            AudioController.Instance.ChangeMusic(AudioController.Instance.AllNotLoopedSongs[currentSongIndex], false, string.Empty, false, false);
            GetSongInfo();
        }

        /// <summary>
        /// Increments song ellapsed time and update UI
        /// </summary>
        private void ShowEllapsedSongTime()
        {
            songEllapsedTime += Time.deltaTime;
            UpdateSongEllapsedTimeText();
        }

        /// <summary>
        /// Recover some information about the song
        /// </summary>
        private void GetSongInfo()
        {
            try
            {
                AudioClip currentSong = AudioController.Instance.AllNotLoopedSongs[currentSongIndex];
                Track track = AudioController.Instance.Tracks.Find(t => t.FileName.Equals(currentSong.name));
                trackNameLabel.text = track.Title;
                artistNameLabel.text = track.Artist;
                songDuration = currentSong.length;
                totalDurationLabel.text = Formatter.GetEllapsedTimeInMinutes((int)songDuration);
                trackSlider.maxValue = songDuration;
                UpdateSongEllapsedTimeText();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the song ellapsed time text
        /// </summary>
        private void UpdateSongEllapsedTimeText()
        {
            trackSlider.value = songEllapsedTime;
            string songEllapsedTimeText = Formatter.GetEllapsedTimeInMinutes((int)songEllapsedTime);
            currentDurationLabel.text = songEllapsedTimeText;

            if (songEllapsedTime >= songDuration)
            {
                if (isSongRepeated)
                {
                    songEllapsedTime = 0;
                }
                else
                {
                    AudioController.Instance.IsSongPlaying = false;
                    AudioController.Instance.StopMusic();
                }
            }
        }

        /// <summary>
        /// Updates the current hour text
        /// </summary>
        private void UpdateHourText() => hourLabel.text = DateTime.Now.ToString("HH:mm");

        /// <summary>
        /// Updates battery level
        /// </summary>
        private void UpdateBatteryLevel() => batteryLevelLabel.text = string.Format("{0}%", (Mathf.FloorToInt(SystemInfo.batteryLevel * 100)));

        /// <summary>
        /// Stop all and calls main menu scene
        /// </summary>
        private IEnumerator CallNextScene()
        {
            canvasGroup.interactable = false;
            actualGameState = Enumerators.GameStates.TRANSITION;
            AudioController.Instance.StopMusic();

            if (isMuted)
            {
                AudioController.Instance.AudioSourceBGM.mute = isMuted;
            }

            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(FadeEffect.Instance.GetFadeOutLength());
            GameStatusController.Instance.NextSceneName = SceneManagerController.SceneNames.SelectLevels;
            GameStatusController.Instance.CameFromLevel = false;
            GameStatusController.Instance.IsLevelCompleted = false;
            SceneManagerController.CallScene(SceneManagerController.SceneNames.Loading);
        }
    }
}