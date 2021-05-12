using System.Collections;
using UnityEngine;

namespace Controllers.Core
{
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

        private const float MAX_BGM_VOLUME = 1f;
        private const float MAX_ME_VOLUME = 1f;
        private const float MAX_SFX_VOLUME = 1f;
        private const float VOLUME_INCREMENT = 0.1f;

        // State
        private AudioClip nextMusic;
        private bool changeScene;
        private string nextSceneName;
        private bool changeOnMusicEnd = false;
        private bool isSongPlaying = false;
        private bool loopMusic = false;

        public float MaxMEVolume => MAX_ME_VOLUME;

        public float MaxSFXVolume => MAX_SFX_VOLUME;

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

        public static AudioController Instance { get; private set; }

        private void Awake() => SetupSingleton();

        private void SetupSingleton()
        {
            int numberOfInstances = FindObjectsOfType(GetType()).Length;
            if (numberOfInstances > 1)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Play one shot of clip
        public void PlaySFX(AudioClip clip, float volume)
        {
            float temporaryVolume = (volume > MAX_SFX_VOLUME ? MAX_SFX_VOLUME : volume);
            audioSourceSFX.volume = temporaryVolume;
            audioSourceSFX.PlayOneShot(clip);
        }

        // Play clip at point
        public void PlaySoundAtPoint(AudioClip clip, float volume)
        {
            if (audioSourceSFX.mute) return;

            float temporaryVolume = (volume > MAX_SFX_VOLUME ? MAX_SFX_VOLUME : volume);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }

        // Plays Music Effect
        public void PlayME(AudioClip clip, float volume, bool loop)
        {
            float temporaryVolume = (volume > MAX_ME_VOLUME ? MAX_ME_VOLUME : volume);
            audioSourceME.volume = temporaryVolume;
            audioSourceME.clip = clip;
            audioSourceME.loop = loop;
            audioSourceME.Play();
        }

        public void StopME()
        {
            audioSourceME.Stop();
            audioSourceME.loop = false;
        }

        public float GetClipLength(AudioClip clip) => (clip ? clip.length : 0);

        public void ChangeMusic(AudioClip nextMusic, bool changeScene, string nextSceneName, bool loopMusic, bool changeOnMusicEnd)
        {
            if (!nextMusic) return;

            this.nextMusic = nextMusic;
            this.changeScene = changeScene;
            this.nextSceneName = nextSceneName;
            this.loopMusic = loopMusic;
            this.changeOnMusicEnd = changeOnMusicEnd;

            StartCoroutine(ChangeMusicCoroutine());
        }

        public void StopMusic()
        {
            StopAllCoroutines();
            StartCoroutine(StopMusicCoroutine());
            isSongPlaying = false;
        }

        private IEnumerator ChangeMusicCoroutine()
        {
            // Drops down volume
            for (float volume = MAX_BGM_VOLUME; volume >= 0; volume -= VOLUME_INCREMENT)
            {
                yield return new WaitForSecondsRealtime(VOLUME_INCREMENT);
                audioSourceBGM.volume = volume;
            }

            // Change and play
            isSongPlaying = false;
            audioSourceBGM.volume = 0;
            audioSourceBGM.clip = nextMusic;
            audioSourceBGM.loop = loopMusic;
            audioSourceBGM.Play();
            isSongPlaying = true;

            // Drops up volume
            for (float volume = 0; volume <= MAX_BGM_VOLUME; volume += VOLUME_INCREMENT)
            {
                yield return new WaitForSecondsRealtime(VOLUME_INCREMENT);
                audioSourceBGM.volume = volume;
            }

            if (!loopMusic && changeOnMusicEnd)
            {
                yield return new WaitForSecondsRealtime(audioSourceBGM.clip.length);
                int index = Random.Range(0, allNotLoopedSongs.Length);
                ChangeMusic(allNotLoopedSongs[index], false, string.Empty, false, true);
            }
        }

        private IEnumerator StopMusicCoroutine()
        {
            // Drops down volume
            for (float volume = MAX_BGM_VOLUME; volume >= 0; volume -= VOLUME_INCREMENT)
            {
                yield return new WaitForSecondsRealtime(VOLUME_INCREMENT);
                audioSourceBGM.volume = volume;
            }

            // Change and play
            audioSourceBGM.volume = 0;
            audioSourceBGM.Stop();
        }
    }
}