using Controllers.Core;
using Effects;
using MVC.Global;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Menu
{
    /// <summary>
    /// Controller for Logo Screen
    /// </summary>
    public class LogoController : MonoBehaviour
    {
        [Header("Needed UI Elements")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image logoImage;

        // || Config

        private const float TIME_TO_CALL_ANIMATION = 1f;
        private const float TIME_TO_WAIT = 2f;
        private const float ALPHA_INCREMENT = 0.1f;

        // || State

        private bool isDatabaseOk = false;

        private void Awake() => UnityUtilities.DisableAnalytics();

        private IEnumerator Start()
        {
            yield return ExtractDatabase();
            yield return PlayAndShowLogo();
        }

        /// <summary>
        /// Extract database file
        /// </summary>
        private IEnumerator ExtractDatabase()
        {
            if (!FileManager.Exists(Configuration.Properties.DatabasePath))
            {
                if (Application.isEditor)
                {
                    FileManager.Copy(Configuration.Properties.DatabaseStreamingAssetsPath, Configuration.Properties.DatabasePath);
                }
                else
                {
                    yield return CopyDatabase();
                }
            }

            isDatabaseOk = true;
            yield return null;
        }

        private IEnumerator CopyDatabase()
        {
            yield return API.API.Get(Configuration.Properties.MobileDatabasePath, (bytes) => FileManager.WriteAllBytes(Configuration.Properties.DatabasePath, bytes));
        }

        /// <summary>
        /// Extract database file
        /// </summary>
        private IEnumerator PlayAndShowLogo()
        {
            yield return new WaitUntil(() => isDatabaseOk);
            yield return new WaitForSecondsRealtime(TIME_TO_CALL_ANIMATION);

            float alpha = iconImage.color.a;
            for (float i = alpha; i < 1f; i += ALPHA_INCREMENT)
            {
                Color color = backgroundImage.color;
                color.a = i;
                backgroundImage.color = color;
                yield return new WaitForSeconds(ALPHA_INCREMENT);
            }

            AudioController.Instance.PlayME(AudioController.Instance.RetrogemnVoice, 1f, false);

            alpha = iconImage.color.a;
            for (float i = alpha; i < 1f; i += ALPHA_INCREMENT)
            {
                Color color = iconImage.color;
                color.a = i;
                iconImage.color = color;
                logoImage.color = color;
                yield return new WaitForSeconds(ALPHA_INCREMENT);
            }

            yield return new WaitForSecondsRealtime(TIME_TO_WAIT);
            yield return new WaitUntil(() => LocalizationController.Instance != null);
            LocalizationController.Instance.GetLocalization();
            yield return new WaitUntil(() => LocalizationController.Instance.DictionaryCount > 0);

            yield return CallNextScene();
        }

        /// <summary>
        /// Goto loading and title
        /// </summary>
        private IEnumerator CallNextScene()
        {
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(FadeEffect.Instance.GetFadeOutLength());
            GameStatusController.Instance.NextSceneName = SceneManagerController.SelectLevelsSceneName;
            GameStatusController.Instance.CameFromLevel = false;
            GameStatusController.Instance.IsLevelCompleted = false;
            SceneManagerController.CallScene(SceneManagerController.LoadingSceneName);
        }
    }
}