using System;
using System.Collections.Generic;

namespace Utilities
{
    [Serializable]
    public class PlayerProgress
    {
        // Data
        private int currentLevelIndex;
        private bool hasPlayerFinishedGame = false;

        public int CurrentLevelIndex { get => currentLevelIndex; set => currentLevelIndex = value; }
        public int TotalNumberOfLevels => 100;
        public bool HasPlayerFinishedGame { get => hasPlayerFinishedGame; set => hasPlayerFinishedGame = value; }

        public PlayerProgress()
        {
            CurrentLevelIndex = 0;
            HasPlayerFinishedGame = false;
        }
    }
}