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
        private static GameStatusController instance;

        public string GetNextSceneName()
        {
            return nextSceneName;
        }

        public int GetLevelIndex()
        {
            return levelIndex;
        }

        public int GetNewScore()
        {
            return newScore;
        }

        public int GetNewTimeScore()
        {
            return newTimeScore;
        }

        public int GetOldScore()
        {
            return oldScore;
        }

        public int GetOldTimeScore()
        {
            return oldTimeScore;
        }

        public bool GetCameFromLevel()
        {
            return cameFromLevel;
        }

        public bool GetHasStartedSong()
        {
            return hasStartedSong;
        }

        public bool GetIsLevelCompleted()
        {
            return isLevelCompleted;
        }


        public void SetNextSceneName(string sceneName)
        {
            this.nextSceneName = sceneName;
        }

        public void SetLevelIndex(int levelIndex)
        {
            this.levelIndex = levelIndex;
        }

        public void SetNewScore(int newScore)
        {
            this.newScore = newScore;
        }

        public void SetNewTimeScore(int newTimeScore)
        {
            this.newTimeScore = newTimeScore;
        }

        public void SetOldScore(int oldScore)
        {
            this.oldScore = oldScore;
        }

        public void SetOldTimeScore(int oldTimeScore)
        {
            this.oldTimeScore = oldTimeScore;
        }

        public void SetCameFromLevel(bool cameFromLevel)
        {
            this.cameFromLevel = cameFromLevel;
        }

        public void SetHasStartedSong(bool hasStartedSong)
        {
            this.hasStartedSong = hasStartedSong;
        }

        public void SetIsLevelCompleted(bool isLevelCompleted)
        {
            this.isLevelCompleted = isLevelCompleted;
        }

        public static GameStatusController Instance { get => instance; }

        private void Awake()
        {
            SetupSingleton();
        }

        private void SetupSingleton()
        {
            int numberOfInstances = FindObjectsOfType(GetType()).Length;
            if (numberOfInstances > 1)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}