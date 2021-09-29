using Controllers.Core;
using UnityEngine;

namespace Core
{
    public class PersistentData : MonoBehaviour
    {
        // || State 
        private int startingSceneIndex;

        // || Properties

        public static PersistentData Instance { get; private set; }

        private void Awake() => SetupSingleton();

        private void Start() => startingSceneIndex = SceneManagerController.GetActiveSceneIndex();

        private void Update()
        {
            int currentSceneIndex = SceneManagerController.GetActiveSceneIndex();
            if (currentSceneIndex != startingSceneIndex)
            {
                DestroyInstance();
            }
        }

        /// <summary>
        /// Setup singleton instance
        /// </summary>
        private void SetupSingleton()
        {
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                DestroyInstance();
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Destroy current instance
        /// </summary>
        public void DestroyInstance() => Destroy(gameObject);
    }
}