using System.Collections;
using UnityEngine;

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

    private float maxBGMVolume = 1f;
    private float maxMEVolume = 1f;
    private float maxSFXVolume = 1f;

    // State
    private AudioClip nextMusic;
    private bool changeScene;
    private string nextSceneName;
    private bool changeOnMusicEnd = false;
    private bool isSongPlaying = false;
    private bool loopMusic = false;
    private static AudioController instance;

    public bool GetIsSongPlaying()
    {
        return isSongPlaying;
    }

    public float GetMaxBGMVolume()
    {
        return maxBGMVolume;
    }

    public float GetMaxMEVolume()
    {
        return maxMEVolume;
    }

    public float GetMaxSFXVolume()
    {
        return maxSFXVolume;
    }

    public void SetIsSongPlaying(bool isSongPlaying)
    {
        this.isSongPlaying = isSongPlaying;
    }

    public void SetMaxBGMVolume(float volume)
    {
        this.maxBGMVolume = volume;
    }

    public void SetMaxMEVolume(float volume)
    {
        this.maxMEVolume = volume;
    }

    public void SetMaxSFXVolume(float volume)
    {
        this.maxSFXVolume = volume;
    }

    // BGM
    public AudioClip SelectLevelsSong { get => selectLevelsSong; }
    public AudioClip[] AllNotLoopedSongs { get => allNotLoopedSongs; }

    // ME
    public AudioClip RetrogemnVoice { get => retrogemnVoice; }
    public AudioClip FireEffect { get => fireEffect; }
    public AudioClip NewScoreEffect { get => newScoreEffect; }
    public AudioClip SuccessEffect { get => successEffect; }

    // SFX
    public AudioClip BlipSound { get => blipSound; }
    public AudioClip BoomSound { get => boomSound; }
    public AudioClip ExplosionSound { get => explosionSound; }
    public AudioClip HittingFace { get => hittingFace; }
    public AudioClip HittingWall { get => hittingWall; }
    public AudioClip LaserPewSound { get => laserPewSound; }
    public AudioClip PowerUpSound { get => powerUpSound; }
    public AudioClip ShowUpSound { get => showUpSound; }
    public AudioClip SlamSound { get => slamSound; }

    public static AudioController Instance { get => instance; }

    private void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        int numberOfInstances = FindObjectsOfType(GetType()).Length;
        if (numberOfInstances > 1)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Play one shot of clip
    public void PlaySFX(AudioClip clip, float volume)
    {
        if (!audioSourceSFX || !clip) return;

        float temporaryVolume = (volume > maxSFXVolume ? maxSFXVolume : volume);
        audioSourceSFX.volume = temporaryVolume;
        audioSourceSFX.PlayOneShot(clip);
    }

    // Play clip at point
    public void PlaySoundAtPoint(AudioClip clip, float volume)
    {
        if (!clip || audioSourceSFX.mute) return;

        float temporaryVolume = (volume > maxSFXVolume ? maxSFXVolume : volume);
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    // Plays Music Effect
    public void PlayME(AudioClip clip, float volume, bool loop)
    {
        // Checks and cancel
        if (!audioSourceME || !clip) return;

        float temporaryVolume = (volume > maxMEVolume ? maxMEVolume : volume);
        audioSourceME.volume = temporaryVolume;
        audioSourceME.clip = clip;
        audioSourceME.loop = loop;
        audioSourceME.Play();
    }

    public void StopME()
    {
        if (!audioSourceME) return;

        audioSourceME.Stop();
        audioSourceME.loop = false;
    }

    // Get clip length
    public float GetClipLength(AudioClip clip)
    {
        return (clip ? clip.length : 0);
    }

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
        if (!audioSourceBGM) { yield return null; }

        // Drops down volume
        for (float volume = maxBGMVolume; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
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
        for (float volume = 0; volume <= maxBGMVolume; volume += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            audioSourceBGM.volume = volume;
        }

        if (!loopMusic && changeOnMusicEnd)
        {
            // Cancel
            yield return new WaitForSecondsRealtime(audioSourceBGM.clip.length);
            int index = Random.Range(0, allNotLoopedSongs.Length);
            ChangeMusic(allNotLoopedSongs[index], false, "", false, true);
        }
    }

    private IEnumerator StopMusicCoroutine()
    {
        if (!audioSourceBGM) { yield return null; }

        // Drops down volume
        for (float volume = maxBGMVolume; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            audioSourceBGM.volume = volume;
        }

        // Change and play
        audioSourceBGM.volume = 0;
        audioSourceBGM.Stop();
    }
}