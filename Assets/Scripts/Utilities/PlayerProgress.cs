using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProgress
{
    // Data
    private const int TOTAL_NUMBER_OF_LEVELS = 100;
    private int currentLevelIndex;
    private bool hasPlayerFinishedGame = false;
    private List<string> levelNamesList = new List<string> ();
    private List<bool> isLevelUnlockedList = new List<bool> ();
    private List<bool> isLevelCompletedList = new List<bool> ();
    private List<int> highScoresList = new List<int> ();
    private List<int> highTimeScoresList = new List<int> ();

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS

    public int GetCurrentLevelIndex () { return currentLevelIndex; }
    public int GetTotalNumberOfLevels () { return TOTAL_NUMBER_OF_LEVELS; }
    public bool GetHasPlayerFinishedGame () { return hasPlayerFinishedGame; }
    public List<int> GetHighScoresList () { return highScoresList; }
    public List<int> GetHighTimeScoresList () { return highTimeScoresList; }
    public List<bool> GetIsLevelCompletedList () { return isLevelCompletedList; }
    public List<bool> GetIsLevelUnlockedList () { return isLevelUnlockedList; }
    public List<string> GetLevelNamesList () { return levelNamesList; }

    public void SetCurrentLevelIndex (int currentLevelIndex) { this.currentLevelIndex = currentLevelIndex; }
    public void SetHasPlayerFinishedGame (bool hasPlayerFinishedGame) { this.hasPlayerFinishedGame = hasPlayerFinishedGame; }
    public void SetHighScoresList (List<int> highScoresList) { this.highScoresList = highScoresList; }
    public void SetHighTimeScoresList (List<int> highTimeScoresList) { this.highTimeScoresList = highTimeScoresList; }
    public void SetIsLevelCompletedList (List<bool> isLevelCompletedList) { this.isLevelCompletedList = isLevelCompletedList; }
    public void SetIsLevelUnlockedList (List<bool> isLevelUnlockedList) { this.isLevelUnlockedList = isLevelUnlockedList; }

    //--------------------------------------------------------------------------------//

    // Constructor with default values
    public PlayerProgress ()
    {
        currentLevelIndex = 0;
        hasPlayerFinishedGame = false;

        for (int i = 0; i < TOTAL_NUMBER_OF_LEVELS; i++)
        {
            isLevelUnlockedList.Add ((i == 0 ? true : false));
            isLevelCompletedList.Add (false);
            highScoresList.Add (0);
            highTimeScoresList.Add (0);
        }

        FillLevelNamesList ();
    }

    //--------------------------------------------------------------------------------//

    private void FillLevelNamesList ()
    {
        // Normal
        levelNamesList.Clear ();
        for (int index = 1; index <= TOTAL_NUMBER_OF_LEVELS; index++) 
        {
            string levelName = string.Concat ("Level", "_", index.ToString ("00"));
            levelNamesList.Add (levelName);
        }
    }
}