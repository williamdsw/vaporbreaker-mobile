using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoController : MonoBehaviour
{
    // Config
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image logoImage;

    // State
    private float timeToCallAnimation = 1f;
    private float timeToWait = 2f;
    private const float INCREMENT = 0.1f;

    // Cached
    private FadeEffect fadeEffect;

    private void Start()
    {
        UnityUtilities.DisableAnalytics();

        // Find Objects
        fadeEffect = FindObjectOfType<FadeEffect>();
        StartCoroutine(PlayAndShowLogo());
    }

    private IEnumerator PlayAndShowLogo()
    {
        // Cancels
        if (!AudioController.Instance) { yield return null; }

        // Plays Logo Sound
        yield return new WaitForSecondsRealtime(timeToCallAnimation);

        // Show background
        if (backgroundImage)
        {
            float alpha = iconImage.color.a;
            for (float i = alpha; i < 1f; i += INCREMENT)
            {
                Color color = backgroundImage.color;
                color.a = i;
                backgroundImage.color = color;
                yield return new WaitForSeconds(INCREMENT);
            }
        }

        // Shows icon & logo
        if (iconImage || logoImage)
        {
            AudioController.Instance.PlayME(AudioController.Instance.RetrogemnVoice, 1f, false);
            float alpha = iconImage.color.a;
            for (float i = alpha; i < 1f; i += INCREMENT)
            {
                Color color = iconImage.color;
                color.a = i;
                iconImage.color = color;
                logoImage.color = color;
                yield return new WaitForSeconds(INCREMENT);
            }
        }

        yield return new WaitForSecondsRealtime(timeToWait);
        StartCoroutine(CallNextScene(SceneManagerController.SelectLevelsSceneName));
    }

    private IEnumerator CallNextScene(string nextSceneName)
    {
        // Cancels
        if (!GameStatusController.Instance || !fadeEffect) { yield return null; }

        float fadeOutLength = fadeEffect.GetFadeOutLength();
        fadeEffect.FadeToLevel();
        yield return new WaitForSecondsRealtime(fadeOutLength);
        GameStatusController.Instance.SetNextSceneName(nextSceneName);
        GameStatusController.Instance.SetCameFromLevel(false);
        GameStatusController.Instance.SetIsLevelCompleted(false);
        SceneManagerController.CallScene(SceneManagerController.LoadingSceneName);
    }
}