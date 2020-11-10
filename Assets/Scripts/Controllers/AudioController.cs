using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Config
    [Header ("Audio Sources")]
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private AudioSource audioSourceME;
    [SerializeField] private AudioSource audioSourceSFX;

    [Header ("BGM")]
    [SerializeField] private AudioClip selectLevelsSong;
    [SerializeField] private AudioClip[] allNotLoopedSongs;

    [Header ("ME")]
    [SerializeField] private AudioClip retrogemnVoice;
    [SerializeField] private AudioClip fireEffect;
    [SerializeField] private AudioClip newScoreEffect;
    [SerializeField] private AudioClip successEffect;

    [Header ("SFX")]
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

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS

    public bool GetIsSongPlaying () { return isSongPlaying; }
    public float GetMaxBGMVolume () { return maxBGMVolume; }
    public float GetMaxMEVolume () { return maxMEVolume; }
    public float GetMaxSFXVolume () { return maxSFXVolume; }

    public void SetIsSongPlaying (bool isSongPlaying) { this.isSongPlaying = isSongPlaying; }
    public void SetMaxBGMVolume (float volume) { this.maxBGMVolume = volume; }
    public void SetMaxMEVolume (float volume) { this.maxMEVolume = volume; }
    public void SetMaxSFXVolume (float volume) { this.maxSFXVolume = volume; }

    //--------------------------------------------------------------------------------//
    // PROPERTIES

    // BGM
    public AudioClip SelectLevelsSong { get { return selectLevelsSong; } }
    public AudioClip[] AllNotLoopedSongs { get { return allNotLoopedSongs; } }

    // ME
    public AudioClip RetrogemnVoice { get { return retrogemnVoice; } }
    public AudioClip FireEffect { get { return fireEffect; } }
    public AudioClip NewScoreEffect { get { return newScoreEffect; } }
    public AudioClip SuccessEffect { get { return successEffect; } }

    // SFX
    public AudioClip BlipSound { get { return blipSound; } }
    public AudioClip BoomSound { get { return boomSound; } }
    public AudioClip ExplosionSound { get { return explosionSound; } }
    public AudioClip HittingFace { get { return hittingFace; } }
    public AudioClip HittingWall { get { return hittingWall; } }
    public AudioClip LaserPewSound { get { return laserPewSound; } }
    public AudioClip PowerUpSound { get { return powerUpSound; } }
    public AudioClip ShowUpSound { get { return showUpSound; } }
    public AudioClip SlamSound { get { return slamSound; } }

    // SINGLETON
    public static AudioController Instance { get { return instance; }}

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Awake () 
    {
        SetupSingleton ();
    }

    //--------------------------------------------------------------------------------//
    // HELPERS

    // Implements singleton
    private void SetupSingleton ()
    {
        int numberOfInstances = FindObjectsOfType (GetType ()).Length;
        if (numberOfInstances > 1)
        {
            DestroyImmediate (this.gameObject);
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad (this.gameObject);
        }
    }

    //--------------------------------------------------------------------------------//
    // SFX FUNCTIONS

    // Play one shot of clip
    public void PlaySFX (AudioClip clip, float volume)
    {
        // Checks and cancel
        if (!audioSourceSFX || !clip) { return; }

        float temporaryVolume = (volume > maxSFXVolume ? maxSFXVolume : volume);
        audioSourceSFX.volume = temporaryVolume;
        audioSourceSFX.PlayOneShot (clip);
    }

    // Play clip at point
    public void PlaySoundAtPoint (AudioClip clip, float volume)
    {
        // Checks and cancel
        if (!clip || audioSourceSFX.mute) { return; }

        float temporaryVolume = (volume > maxSFXVolume ? maxSFXVolume : volume);
        AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position, volume);
    }

    //--------------------------------------------------------------------------------//
    // ME FUNCTIONS

    // Plays Music Effect
    public void PlayME (AudioClip clip, float volume, bool loop)
    {
        // Checks and cancel
        if (!audioSourceME || !clip) { return; }

        float temporaryVolume = (volume > maxMEVolume ? maxMEVolume : volume);
        audioSourceME.volume = temporaryVolume;
        audioSourceME.clip = clip;
        audioSourceME.loop = loop;
        audioSourceME.Play ();
    }

    public void StopME ()
    {
        // Checks and cancel
        if (!audioSourceME) { return; }

        audioSourceME.Stop ();
        audioSourceME.loop = false;
    }

    //--------------------------------------------------------------------------------//
    // CLIP / SONG PROPERTIES

    // Get clip length
    public float GetClipLength (AudioClip clip)
    {
        // Checks and cancel
        if (!clip) { return 0; }
        return clip.length;
    }
    
    //--------------------------------------------------------------------------------//
    // BGM

    public void ChangeMusic (AudioClip nextMusic, bool changeScene, string nextSceneName, bool loopMusic, bool changeOnMusicEnd)
    {
        // Checks and cancel
        if (!nextMusic) { return; }

        this.nextMusic = nextMusic;
        this.changeScene = changeScene;
        this.nextSceneName = nextSceneName;
        this.loopMusic = loopMusic;
        this.changeOnMusicEnd = changeOnMusicEnd;

        StartCoroutine (ChangeMusicCoroutine ());
    }

    public void StopMusic ()
    {
        StopAllCoroutines ();
        StartCoroutine (StopMusicCoroutine ());
        isSongPlaying = false;
    }

    //--------------------------------------------------------------------------------//
    // COROUTINES

    private IEnumerator ChangeMusicCoroutine ()
    {
        // Checks and cancel
        if (!audioSourceBGM) { yield return null; }

        // Drops down volume
        for (float volume = maxBGMVolume; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            audioSourceBGM.volume = volume;
        }

        // Change and play
        isSongPlaying = false;
        audioSourceBGM.volume = 0;
        audioSourceBGM.clip = nextMusic;
        audioSourceBGM.loop = loopMusic;
        audioSourceBGM.Play ();
        isSongPlaying = true;

        // Drops up volume
        for (float volume = 0; volume <= maxBGMVolume; volume += 0.1f)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            audioSourceBGM.volume = volume;
        }

        if (!loopMusic && changeOnMusicEnd)
        {
            // Cancel
            yield return new WaitForSecondsRealtime (audioSourceBGM.clip.length);
            int index = Random.Range (0, allNotLoopedSongs.Length);
            ChangeMusic (allNotLoopedSongs[index], false, "", false, true);
        }
    }

    private IEnumerator StopMusicCoroutine ()
    {
        // Checks and cancel
        if (!audioSourceBGM) { yield return null; }
        
        // Drops down volume
        for (float volume = maxBGMVolume; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            audioSourceBGM.volume = volume;
        }

        // Change and play
        audioSourceBGM.volume = 0;
        audioSourceBGM.Stop ();
    }
}