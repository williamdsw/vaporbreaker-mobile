using System.Collections;
using System.Collections.Generic;
using MVC.BL;
using MVC.Models;
using UnityEngine;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for Audio
    /// </summary>
    public class AudioController : MonoBehaviour
    {
        // Config
        [Header("Audio Sources")]
        [SerializeField] private AudioSource audioSourceBGM;
        [SerializeField] private AudioSource audioSourceME;
        [SerializeField] private AudioSource audioSourceSFX;

        [Header("BGM")]
        [SerializeField] private AudioClip selectLevelsSong;
        [SerializeField] private AudioClip[] allNotLoopedSongs;

        [Header("ME")]
        [SerializeField] private AudioClip retrogemnVoice;
        [SerializeField] private AudioClip fireEffect;
        [SerializeField] private AudioClip newScoreEffect;
        [SerializeField] private AudioClip successEffect;

        [Header("SFX")]
        [SerializeField] private AudioClip blipSound;
        [SerializeField] private AudioClip boomSound;
        [SerializeField] private AudioClip explosionSound;
        [SerializeField] private AudioClip hittingFace;
        [SerializeField] private AudioClip hittingWall;
        [SerializeField] private AudioClip laserPewSound;
        [SerializeField] private AudioClip powerUpSound;
        [SerializeField] private AudioClip showUpSound;
        [SerializeField] private AudioClip slamSound;

        // || Const

        private const float VOLUME_INCREMENT = 0.1f;

        // || State

        private AudioClip nextTrackClip;
        private string nextSceneName;
        private bool isToChangeScene;
        private bool isToChangeOnTrackEnd = false;
        private bool isToLoopTrack = false;

        // || Cached

        private TrackBL trackBL;

        // || Properties

        public static AudioController Instance { get; private set; }

        // || Config

        public float MaxBGMVolume { get; set; } = 1f;
        public float MaxMEVolume { get; set; } = 1f;
        public float MaxSFXVolume { get; set; } = 1f;

        // BGM
        public AudioClip SelectLevelsSong => selectLevelsSong;
        public AudioClip[] AllNotLoopedSongs => allNotLoopedSongs;

        // ME
        public AudioClip RetrogemnVoice => retrogemnVoice;
        public AudioClip FireEffect => fireEffect;
        public AudioClip NewScoreEffect => newScoreEffect;
        public AudioClip SuccessEffect => successEffect;

        // SFX
        public AudioClip BlipSound => blipSound;
        public AudioClip BoomSound => boomSound;
        public AudioClip ExplosionSound => explosionSound;
        public AudioClip HittingFace => hittingFace;
        public AudioClip HittingWall => hittingWall;
        public AudioClip LaserPewSound => laserPewSound;
        public AudioClip PowerUpSound => powerUpSound;
        public AudioClip ShowUpSound => showUpSound;
        public AudioClip SlamSound => slamSound;

        // || Others

        public AudioSource AudioSourceBGM => audioSourceBGM;
        public AudioSource AudioSourceME => audioSourceME;
        public AudioSource AudioSourceSFX => audioSourceSFX;

        public List<Track> Tracks { get; private set; }
        public bool IsSongPlaying { get; set; }

        private void Awake()
        {
            SetupSingleton();
            trackBL = new TrackBL();
            Tracks = new List<Track>();
        }

        /// <summary>
        /// Setup singleton instance
        /// </summary>
        private void SetupSingleton()
        {
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Play SFX at volume
        /// </summary>
        /// <param name="clip"> Clip to be played </param>
        /// <param name="volume"> Volume amount </param>
        public void PlaySFX(AudioClip clip, float volume)
        {
            if (AudioSourceSFX.mute || AudioSourceSFX.isPlaying) return;
            float temporaryVolume = (volume > MaxSFXVolume ? MaxSFXVolume : volume);
            AudioSourceSFX.volume = temporaryVolume;
            AudioSourceSFX.PlayOneShot(clip);
        }

        /// <summary>
        /// Play ME at volume with loop
        /// </summary>
        /// <param name="clip"> Clip to be played </param>
        /// <param name="volume"> Volume amount </param>
        /// <param name="toLoop"> Is to loop ? </param>
        public void PlayME(AudioClip clip, float volume, bool toLoop)
        {
            if (AudioSourceME.mute || AudioSourceME.isPlaying) return;
            float temporaryVolume = (volume > MaxMEVolume ? MaxMEVolume : volume);
            AudioSourceME.volume = temporaryVolume;
            AudioSourceME.clip = clip;
            AudioSourceME.loop = toLoop;
            AudioSourceME.Play();
        }

        /// <summary>
        /// Stops the current ME
        /// </summary>
        public void StopME()
        {
            AudioSourceME.Stop();
            audioSourceME.loop = false;
        }

        /// <summary>
        /// Get the duration of a track
        /// </summary>
        /// <param name="clip"></param>
        /// <returns> Duration of the track </returns>
        public float GetClipLength(AudioClip clip) => (clip ? clip.length : 0f);

        /// <summary>
        /// Pass values to change music
        /// </summary>
        /// <param name="nextTrackClip"> Next track to be played </param>
        /// <param name="isToChangeScene"> Is to change scene ? </param>
        /// <param name="nextSceneName"> Next scene name </param>
        /// <param name="isToLoopTrack"> Is to loop current track ? </param>
        /// <param name="isToChangeOnTrackEnd"> Is to change on track ending ? </param>
        public void ChangeMusic(AudioClip nextTrackClip, bool isToChangeScene, string nextSceneName, bool isToLoopTrack, bool isToChangeOnTrackEnd)
        {
            this.nextTrackClip = nextTrackClip;
            this.isToChangeScene = isToChangeScene;
            this.nextSceneName = nextSceneName;
            this.isToLoopTrack = isToLoopTrack;
            this.isToChangeOnTrackEnd = isToChangeOnTrackEnd;

            StartCoroutine(ChangeMusicCoroutine());
        }

        /// <summary>
        /// Do all process to change current track
        /// </summary>
        private IEnumerator ChangeMusicCoroutine()
        {
            yield return DropVolume();

            IsSongPlaying = false;
            AudioSourceBGM.volume = 0;
            AudioSourceBGM.clip = nextTrackClip;
            AudioSourceBGM.loop = isToLoopTrack;
            AudioSourceBGM.Play();
            IsSongPlaying = true;

            yield return GainVolume();

            if (!isToLoopTrack && isToChangeOnTrackEnd)
            {
                yield return new WaitForSecondsRealtime(AudioSourceBGM.clip.length);
                int index = Random.Range(0, allNotLoopedSongs.Length);
                ChangeMusic(allNotLoopedSongs[index], false, string.Empty, false, true);
            }
        }

        /// <summary>
        /// Fade in volume to zero
        /// </summary>
        private IEnumerator DropVolume()
        {
            for (float volume = MaxBGMVolume; volume >= 0; volume -= 0.1f)
            {
                AudioSourceBGM.volume = volume;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        /// <summary>
        /// Fade out volume to MaxBGMVolume
        /// </summary>
        private IEnumerator GainVolume()
        {
            for (float volume = 0; volume <= MaxBGMVolume; volume += 0.1f)
            {
                AudioSourceBGM.volume = volume;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        /// <summary>
        /// Pause or unpause current track
        /// </summary>
        /// <param name="isToPause"> Is to pause current track ? </param>
        public void PauseMusic(bool isToPause)
        {
            if (isToPause)
            {
                AudioSourceBGM.Pause();
            }
            else
            {
                AudioSourceBGM.UnPause();
            }
        }

        /// <summary>
        /// Toggle track's loop property
        /// </summary>
        /// <param name="isToRepeat"> Is to repeat current track ? </param>
        public void ToggleRepeatTrack(bool isToRepeat) => AudioSourceBGM.loop = isToRepeat;

        /// <summary>
        /// Stop current track
        /// </summary>
        public void StopMusic()
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine());
            IsSongPlaying = false;
        }

        /// <summary>
        /// Stop current track
        /// </summary>
        private IEnumerator StopMusicCoroutine()
        {
            yield return DropVolume();

            AudioSourceBGM.volume = 0;
            AudioSourceBGM.Stop();
        }

        /// <summary>
        /// List all tracks
        /// </summary>
        public void GetTracks() => Tracks = trackBL.ListAll();
    }
}