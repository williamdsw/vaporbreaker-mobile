using UnityEngine;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for PlayerPrefs
    /// </summary>
    public class PlayerPrefsController
    {
        private class Keys
        {
            public static string IsBackgroundMusicMute => "IsBackgroundMusicMute";
            public static string IsMusicEffectsMute => "IsMusicEffectsMute";
            public static string IsSoundEffectsMute => "IsSoundEffectsMute";
            public static string HasPlayerPrefs => "HasPlayerPrefs";
            public static string Language => "Language";
        }

        /// <summary>
        /// Has stored player prefs?
        /// </summary>
        public static bool HasPlayerPrefs
        {
            get => !string.IsNullOrEmpty(PlayerPrefs.GetString(Keys.HasPlayerPrefs));
            set
            {
                if (!string.IsNullOrEmpty(value.ToString()) && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    PlayerPrefs.SetString(Keys.HasPlayerPrefs, value.ToString());
                }
            }
        }

        /// <summary>
        /// Is background music muted?
        /// </summary>
        public static string IsBackgroundMusicMute
        {
            get => PlayerPrefs.GetString(Keys.IsBackgroundMusicMute);
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    PlayerPrefs.SetString(Keys.IsBackgroundMusicMute, value);
                }
            }
        }

        /// <summary>
        /// Is music effect muted?
        /// </summary>
        public static string IsMusicEffectsMute
        {
            get => PlayerPrefs.GetString(Keys.IsMusicEffectsMute);
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    PlayerPrefs.SetString(Keys.IsMusicEffectsMute, value);
                }
            }
        }

        /// <summary>
        /// Is sound effects muted?
        /// </summary>
        public static string IsSoundEffectsMute
        {
            get => PlayerPrefs.GetString(Keys.IsSoundEffectsMute);
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    PlayerPrefs.SetString(Keys.IsSoundEffectsMute, value);
                }
            }
        }

        /// <summary>
        /// Current selected language
        /// </summary>
        public static string Language
        {
            get => PlayerPrefs.GetString(Keys.Language);
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    PlayerPrefs.SetString(Keys.Language, value);
                }
            }
        }
    }
}