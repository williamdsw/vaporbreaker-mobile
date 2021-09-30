using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.Core
{
    public class SceneManagerController
    {
        public static string LoadingSceneName => "Loading";
        public static string Level => "Level";
        public static string SelectLevelsSceneName => "SelectLevels";

        public static void CallScene(string sceneName) => SceneManager.LoadScene(sceneName);

        public static AsyncOperation CallSceneAsync(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

        public static int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;

        public static void QuitGame() => Application.Quit();

        public static void ReloadScene() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}