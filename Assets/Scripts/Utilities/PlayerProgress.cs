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
        private List<string> levelNamesList = new List<string>();
        private List<bool> isLevelUnlockedList = new List<bool>();
        private List<bool> isLevelCompletedList = new List<bool>();
        private List<int> highScoresList = new List<int>();
        private List<int> highTimeScoresList = new List<int>();

        public int CurrentLevelIndex { get => currentLevelIndex; set => currentLevelIndex = value; }
        public int TotalNumberOfLevels => 100;
        public bool HasPlayerFinishedGame { get => hasPlayerFinishedGame; set => hasPlayerFinishedGame = value; }
        public List<string> LevelNamesList => levelNamesList;
        public List<bool> IsLevelUnlockedList { get => isLevelUnlockedList; set => isLevelUnlockedList = value; }
        public List<bool> IsLevelCompletedList { get => isLevelCompletedList; set => isLevelCompletedList = value; }
        public List<int> HighScoresList { get => highScoresList; set => highScoresList = value; }
        public List<int> HighTimeScoresList { get => highTimeScoresList; set => highTimeScoresList = value; }

        public PlayerProgress()
        {
            CurrentLevelIndex = 0;
            HasPlayerFinishedGame = false;

            for (int index = 0; index < TotalNumberOfLevels; index++)
            {
                IsLevelUnlockedList.Add((index == 0 ? true : false));
                IsLevelCompletedList.Add(false);
                HighScoresList.Add(0);
                HighTimeScoresList.Add(0);
            }

            FillLevelNamesList();
        }

        private void FillLevelNamesList()
        {
            // Normal
            LevelNamesList.Clear();
            for (int index = 1; index <= TotalNumberOfLevels; index++)
            {
                string levelName = string.Concat("Level", "_", index.ToString("00"));
                LevelNamesList.Add(levelName);
            }
        }
    }
}