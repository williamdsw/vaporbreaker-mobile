using System.Collections.Generic;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    // Menus
    private List<string> aboutLabels = new List<string>();
    private List<string> configurationsLabels = new List<string>();
    private List<string> confirmBoxLabels = new List<string>();
    private List<string> instructions01Labels = new List<string>();
    private List<string> instructions02Labels = new List<string>();
    private List<string> instructions03Labels = new List<string>();
    private List<string> levelCompleteLabels = new List<string>();
    private List<string> levelDetailsLabels = new List<string>();
    private List<string> pauseLabels = new List<string>();
    private List<string> selectLevelsLabels = new List<string>();

    // Singleton
    private static LocalizationController instance;

    // Menus
    public List<string> GetAboutLabels()
    {
        return aboutLabels;
    }

    public List<string> GetConfigurationsLabels()
    {
        return configurationsLabels;
    }

    public List<string> GetConfirmBoxLabels()
    {
        return confirmBoxLabels;
    }

    public List<string> GetInstructions01Labels()
    {
        return instructions01Labels;
    }

    public List<string> GetInstructions02Labels()
    {
        return instructions02Labels;
    }

    public List<string> GetInstructions03Labels()
    {
        return instructions03Labels;
    }

    public List<string> GetLevelCompleteLabels()
    {
        return levelCompleteLabels;
    }

    public List<string> GetLevelDetailsLabels()
    {
        return levelDetailsLabels;
    }

    public List<string> GetPauseLabels()
    {
        return pauseLabels;
    }

    public List<string> GetSelectLevelsLabels()
    {
        return selectLevelsLabels;
    }

    public static LocalizationController Instance { get => instance; }

    private void Awake()
    {
        SetupSingleton();
        DefineLocalization();
    }

    // Implements singleton
    private void SetupSingleton()
    {
        int numberOfInstances = FindObjectsOfType(GetType()).Length;
        if (numberOfInstances > 1)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void ClearLists()
    {
        // Panels
        aboutLabels.Clear();
        configurationsLabels.Clear();
        confirmBoxLabels.Clear();
        instructions01Labels.Clear();
        instructions02Labels.Clear();
        instructions03Labels.Clear();
        levelCompleteLabels.Clear();
        levelDetailsLabels.Clear();
        pauseLabels.Clear();
        selectLevelsLabels.Clear();
    }

    private void DefineLocalization()
    {
        string language = PlayerPrefsController.GetLanguage();
        string folderPath = string.Empty;
        if (string.IsNullOrEmpty(language) || string.IsNullOrWhiteSpace(language))
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                default:
                {
                    language = SystemLanguage.English.ToString(); break;
                }

                case SystemLanguage.Portuguese:
                {
                    language = SystemLanguage.Portuguese.ToString(); break;
                }
            }

            PlayerPrefsController.SetLanguage(language);
        }

        switch (language)
        {
            case "English":
            default:
            {
                folderPath = FileManager.GetLocalizationEnglishFolderPath(); break;
            }

            case "Portuguese":
            {
                folderPath = FileManager.GetLocalizationPortugueseFolderPath(); break;
            }
        }

        LoadLocalization(folderPath);
    }

    public void LoadLocalization(string folderPath)
    {
        ClearLists();

        // Panels
        string uiLabels = FileManager.LoadAsset(folderPath, FileManager.GetUILabelsPath());
        string[] panels = uiLabels.Split('\n');

        // Checks & Cancel
        if (panels.Length == 0) return;

        List<string>[] labelsList =
        {
            selectLevelsLabels, configurationsLabels, aboutLabels, confirmBoxLabels, levelDetailsLabels,
            instructions01Labels, instructions02Labels, instructions03Labels, pauseLabels, levelCompleteLabels
        };

        for (int index = 0; index < panels.Length; index++)
        {
            SplitToLabels(panels[index], labelsList[index]);
        }
    }

    private void SplitToLabels(string line, List<string> fields)
    {
        if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line) || fields == null) return;

        foreach (string label in line.Split('|'))
        {
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrWhiteSpace(label))
            {
                fields.Add(label);
            }
        }
    }
}