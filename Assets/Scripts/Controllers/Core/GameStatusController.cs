using UnityEngine;

namespace Controllers.Core
{
    public class GameStatusController : MonoBehaviour
    {
        // Config
        [SerializeField] private string nextSceneName;
        [SerializeField] private int levelIndex = 0;
        [SerializeField] private int newScore = 0;
        [SerializeField] private int newTimeScore = 0;
        [SerializeField] private int oldScore = 0;
        [SerializeField] private int oldTimeScore = 0;
        [SerializeField] private bool cameFromLevel = false;
        [SerializeField] private bool hasStartedSong = false;
        [SerializeField] private bool isLevelCompleted = false;

        public string NextSceneName { get => nextSceneName; set => nextSceneName = value; }
        public int LevelIndex { get => levelIndex; set => levelIndex = value; }
        public int NewScore { get => newScore; set => newScore = value; }
        public int NewTimeScore { get => newTimeScore; set => newTimeScore = value; }
        public int OldScore { get => oldScore; set => oldScore = value; }
        public int OldTimeScore { get => oldTimeScore; set => oldTimeScore = value; }
        public bool CameFromLevel { get => cameFromLevel; set => cameFromLevel = value; }
        public bool HasStartedSong { get => hasStartedSong; set => hasStartedSong = value; }
        public bool IsLevelCompleted { get => isLevelCompleted; set => isLevelCompleted = value; }
        public static GameStatusController Instance { get; private set; }

        private void Awake() => SetupSingleton();

        private void SetupSingleton()
        {
            int numberOfInstances = FindObjectsOfType(GetType()).Length;
            if (numberOfInstances > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}