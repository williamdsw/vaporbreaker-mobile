using UnityEngine;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for data about current level
    /// </summary>
    public class GameStatusController : MonoBehaviour
    {
        // || Properties

        public static GameStatusController Instance { get; private set; }

        public string NextSceneName { get; set; }
        public int LevelIndex { get; set; }
        public bool CameFromLevel { get; set; }
        public bool HasStartedSong { get; set; }
        public bool IsLevelCompleted { get; set; }
        public long LevelId { get; set; }
        public long NewScore { get; set; }
        public long NewTimeScore { get; set; }
        public long OldScore { get; set; }
        public long OldTimeScore { get; set; }
        public long OldCombo { get; set; }
        public long NewCombo { get; set; }

        private void Awake() => SetupSingleton();

        /// <summary>
        /// Setup singleton instance
        /// </summary>
        private void SetupSingleton()
        {
            if (FindObjectsOfType(GetType()).Length > 1)
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