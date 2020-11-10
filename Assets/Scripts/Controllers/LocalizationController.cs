using System.Collections.Generic;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    // Menus
    private List<string> aboutLabels = new List<string> ();
    private List<string> configurationsLabels = new List<string> ();
    private List<string> confirmBoxLabels = new List<string> ();
    private List<string> instructions01Labels = new List<string> ();
    private List<string> instructions02Labels = new List<string> ();
    private List<string> instructions03Labels = new List<string> ();
    private List<string> levelCompleteLabels = new List<string> ();
    private List<string> levelDetailsLabels = new List<string> ();
    private List<string> pauseLabels = new List<string> ();
    private List<string> selectLevelsLabels = new List<string> ();

    // Singleton
    private static LocalizationController instance;

    //--------------------------------------------------------------------------------//
    // GETTERS

    // Menus
    public List<string> GetAboutLabels () { return aboutLabels; }
    public List<string> GetConfigurationsLabels () { return configurationsLabels; }
    public List<string> GetConfirmBoxLabels () { return confirmBoxLabels; }
    public List<string> GetInstructions01Labels () { return instructions01Labels; }
    public List<string> GetInstructions02Labels () { return instructions02Labels; }
    public List<string> GetInstructions03Labels () { return instructions03Labels; }
    public List<string> GetLevelCompleteLabels () { return levelCompleteLabels; }
    public List<string> GetLevelDetailsLabels () { return levelDetailsLabels; }
    public List<string> GetPauseLabels () { return pauseLabels; }
    public List<string> GetSelectLevelsLabels () { return selectLevelsLabels; }

    //--------------------------------------------------------------------------------//
    // PROPERTIES

    public static LocalizationController Instance { get { return instance; }}

    //--------------------------------------------------------------------------------//

    private void Awake () 
    {
        SetupSingleton ();
        DefineLocalization ();
    }

    //--------------------------------------------------------------------------------//

    // Implements singleton
    private void SetupSingleton ()
    {
        int numberOfInstances = FindObjectsOfType (GetType ()).Length;
        if (numberOfInstances > 1)
        {
            DestroyImmediate (this.gameObject);
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad (this.gameObject);
        }
    }

    //--------------------------------------------------------------------------------//

    private void ClearLists ()
    {
        // Panels
        aboutLabels.Clear ();
        configurationsLabels.Clear ();
        confirmBoxLabels.Clear ();
        instructions01Labels.Clear ();
        instructions02Labels.Clear ();
        instructions03Labels.Clear ();
        levelCompleteLabels.Clear ();
        levelDetailsLabels.Clear ();
        pauseLabels.Clear ();
        selectLevelsLabels.Clear ();
    }

    private void DefineLocalization ()
    {
        string language = PlayerPrefsController.GetLanguage ();
        string folderPath = string.Empty;
        if (string.IsNullOrEmpty (language) || string.IsNullOrWhiteSpace (language))
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English: { language = SystemLanguage.English.ToString (); break;}
                case SystemLanguage.Portuguese: { language = SystemLanguage.Portuguese.ToString (); break;}
                default: { language = SystemLanguage.English.ToString (); break; }
            }

            PlayerPrefsController.SetLanguage (language);
        }

        switch (language)
        {
            case "English": { folderPath = FileManager.GetLocalizationEnglishFolderPath (); break; }
            case "Portuguese": { folderPath = FileManager.GetLocalizationPortugueseFolderPath (); break; }
            default: { folderPath = FileManager.GetLocalizationEnglishFolderPath (); break; }
        }

        LoadLocalization (folderPath);
    }

    public void LoadLocalization (string folderPath)
    {
        ClearLists ();

        // Panels
        string uiLabels = FileManager.LoadAsset (folderPath, FileManager.GetUILabelsPath ());
        string[] panels = uiLabels.Split ('\n');

        // Checks & Cancel
        if (panels.Length == 0) { return; }

        foreach (string label in panels[0].Split ('|')) { if (!string.IsNullOrEmpty (label)) { selectLevelsLabels.Add (label); } }
        foreach (string label in panels[1].Split ('|')) { if (!string.IsNullOrEmpty (label)) { configurationsLabels.Add (label); } }
        foreach (string label in panels[2].Split ('|')) { if (!string.IsNullOrEmpty (label)) { aboutLabels.Add (label); } }
        foreach (string label in panels[3].Split ('|')) { if (!string.IsNullOrEmpty (label)) { confirmBoxLabels.Add (label); } }
        foreach (string label in panels[4].Split ('|')) { if (!string.IsNullOrEmpty (label)) { levelDetailsLabels.Add (label); } }
        foreach (string label in panels[5].Split ('|')) { if (!string.IsNullOrEmpty (label)) { instructions01Labels.Add (label); } }
        foreach (string label in panels[6].Split ('|')) { if (!string.IsNullOrEmpty (label)) { instructions02Labels.Add (label); } }
        foreach (string label in panels[7].Split ('|')) { if (!string.IsNullOrEmpty (label)) { instructions03Labels.Add (label); } }
        foreach (string label in panels[8].Split ('|')) { if (!string.IsNullOrEmpty (label)) { pauseLabels.Add (label); } }
        foreach (string label in panels[9].Split ('|')) { if (!string.IsNullOrEmpty (label)) { levelCompleteLabels.Add (label); } }
    }
}