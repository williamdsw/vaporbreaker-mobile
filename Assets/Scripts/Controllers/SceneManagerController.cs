using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController
{

    public static string LoadingSceneName => "Loading_Screen";

    public static string SelectLevelsSceneName => "Select_Levels";

    public static void CallScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName) || string.IsNullOrWhiteSpace(sceneName)) return;
        SceneManager.LoadScene(sceneName);
    }

    public static AsyncOperation CallSceneAsync(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName) || string.IsNullOrWhiteSpace(sceneName)) return null;
        return SceneManager.LoadSceneAsync(sceneName);
    }

    public static int GetActiveSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void ReloadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}