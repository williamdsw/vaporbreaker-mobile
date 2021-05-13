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
                Destroy(gameObject);
            }
        }

        private void SetupSingleton()
        {
            int numberOfInstances = FindObjectsOfType(GetType()).Length;
            if (numberOfInstances > 1)
            {
                DestroyInstance();
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void DestroyInstance()
        {
            DestroyImmediate(gameObject);
        }
    }
}