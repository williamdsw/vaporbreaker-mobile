﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for Scene Manager
    /// </summary>
    public class SceneManagerController
    {
        public class SceneNames
        {
            // || Properties

            public static string Level => "Level";
            public static string Loading => "Loading";
            public static string SelectLevels => "SelectLevels";
            public static string Soundtrack => "Soundtrack";
        }

        /// <summary>
        /// Calls a scene by name
        /// </summary>
        /// <param name="sceneName"> Valid scene name </param>
        public static void CallScene(string sceneName) => SceneManager.LoadScene(sceneName);

        /// <summary>
        /// Calls a scene asynchronously by name
        /// </summary>
        /// <param name="sceneName"> Valid scene name </param>
        public static AsyncOperation CallSceneAsync(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

        /// <summary>
        /// Get build index of active scene
        /// </summary>
        public static int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;

        /// <summary>
        /// Quits the application
        /// </summary>
        public static void QuitGame() => Application.Quit();

        /// <summary>
        /// Reload actual scene
        /// </summary>
        public static void ReloadScene() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}