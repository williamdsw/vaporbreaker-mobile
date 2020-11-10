using UnityEngine;

public class PlayerPrefsController
{
    // keys
    private const string BGM_VOLUME_MUTE_KEY = "BGM_Volume_Mute_Key";
    private const string HAS_PLAYER_PREFS_KEY = "Has_Player_Prefs";
    private const string LANGUAGE_KEY = "Language";
    private const string ME_VOLUME_MUTE_KEY = "ME_Volume_Mute_Key";
    private const string SFX_VOLUME_MUTE_KEY = "SFX_Volume_Mute_Key";

    //--------------------------------------------------------------------------------//

    // GETTERS / SETTERS

    public static string GetBGMVolumeMute () { return PlayerPrefs.GetString (BGM_VOLUME_MUTE_KEY); }
    public static string GetMEVolumeMute () { return PlayerPrefs.GetString (ME_VOLUME_MUTE_KEY); }
    public static string GetSFXVolumeMute () { return PlayerPrefs.GetString (SFX_VOLUME_MUTE_KEY); }
    public static string GetLanguage () { return PlayerPrefs.GetString (LANGUAGE_KEY); }
    public static bool HasPlayerPrefs () { return !string.IsNullOrEmpty (PlayerPrefs.GetString (HAS_PLAYER_PREFS_KEY)); }

    public static void SetBGMVolumeMute (string isMute)
    {
        if (!string.IsNullOrEmpty (isMute) && !string.IsNullOrWhiteSpace (isMute))
        {
            PlayerPrefs.SetString (BGM_VOLUME_MUTE_KEY, isMute);
        }
    }

    public static void SetMEVolumeMute (string isMute)
    {
        if (!string.IsNullOrEmpty (isMute) && !string.IsNullOrWhiteSpace (isMute))
        {
            PlayerPrefs.SetString (ME_VOLUME_MUTE_KEY, isMute);
        }
    }

    public static void SetSFXVolumeMute (string isMute)
    {
        if (!string.IsNullOrEmpty (isMute) && !string.IsNullOrWhiteSpace (isMute))
        {
            PlayerPrefs.SetString (SFX_VOLUME_MUTE_KEY, isMute);
        }
    }

    public static void SetHasPlayerPrefs (string hasPlayerPrefs)
    {
        if (!string.IsNullOrEmpty (hasPlayerPrefs) && !string.IsNullOrWhiteSpace (hasPlayerPrefs))
        {
            PlayerPrefs.SetString (HAS_PLAYER_PREFS_KEY, hasPlayerPrefs);    
        }
    }

    public static void SetLanguage (string language)
    {
        if (!string.IsNullOrEmpty (language) && !string.IsNullOrWhiteSpace (language))
        {
            PlayerPrefs.SetString (LANGUAGE_KEY, language);    
        }
    }
}