using System;

namespace Utilities
{
    /// <summary>
    /// Progress data for Player
    /// </summary>
    [Serializable]
    public class PlayerProgress
    {
        // || State

        private int currentLevelIndex;
        private bool hasPlayerFinishedGame = false;

        // || Properties

        /// <summary>
        /// Current level index
        /// </summary>
        public int CurrentLevelIndex { get => currentLevelIndex; set => currentLevelIndex = value; }

        /// <summary>
        /// Has player finished the game?
        /// </summary>
        public bool HasPlayerFinishedGame { get => hasPlayerFinishedGame; set => hasPlayerFinishedGame = value; }

        public PlayerProgress()
        {
            CurrentLevelIndex = 0;
            HasPlayerFinishedGame = false;
        }
    }
}