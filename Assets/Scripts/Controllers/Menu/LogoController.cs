using Controllers.Core;
using Effects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Controllers.Menu
{
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

        private void Start()
        {
            UnityUtilities.DisableAnalytics();
            StartCoroutine(PlayAndShowLogo());
        }

        private IEnumerator PlayAndShowLogo()
        {
            // Plays Logo Sound
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
            StartCoroutine(CallNextScene(SceneManagerController.SelectLevelsSceneName));
        }

        private IEnumerator CallNextScene(string nextSceneName)
        {
            float fadeOutLength = FadeEffect.Instance.GetFadeOutLength();
            FadeEffect.Instance.FadeToLevel();
            yield return new WaitForSecondsRealtime(fadeOutLength);
            GameStatusController.Instance.NextSceneName = nextSceneName;
            GameStatusController.Instance.CameFromLevel = false;
            GameStatusController.Instance.IsLevelCompleted = false;
            SceneManagerController.CallScene(SceneManagerController.LoadingSceneName);
        }
    }
}