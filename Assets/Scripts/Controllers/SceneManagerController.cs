using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController
{
    private const string LOADING_SCENE_NAME = "Loading_Screen";
    private const string SELECT_LEVELS_SCENE_NAME = "Select_Levels";

    //--------------------------------------------------------------------------------//
    // GETTERS

    public static string GetLoadingSceneName () { return LOADING_SCENE_NAME; }
    public static string GetSelectLevelsSceneName () { return SELECT_LEVELS_SCENE_NAME; }

    //--------------------------------------------------------------------------------//

    // Calls scene by name
    public static void CallScene (string sceneName)
    {
        SceneManager.LoadScene (sceneName);
    }

    // Calls scene async by name
    public static AsyncOperation CallSceneAsync (string sceneName)
    {
        return SceneManager.LoadSceneAsync (sceneName);
    }

    // Get active scene's index
    public static int GetActiveSceneIndex ()
    {
        return SceneManager.GetActiveScene ().buildIndex;
    }

    // Quits the application
    public static void QuitGame ()
    {
        Application.Quit ();
    }
    
    // Reloads the actual scene
    public static void ReloadScene ()
    {
        int sceneIndex = SceneManager.GetActiveScene ().buildIndex;
        SceneManager.LoadSceneAsync (sceneIndex);
    }
}