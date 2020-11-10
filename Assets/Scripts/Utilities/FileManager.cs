using UnityEngine;

public class FileManager
{
    // Folders
    private const string FILES_FOLDER_PATH = "Files/";
    private const string LEVELS_THUMBNAILS_PATH = "Levels_Thumbnails";
    private const string LOCALIZATION_ENGLISH_FOLDER_PATH = "Localization/en/";
    private const string LOCALIZATION_PORTUGUESE_FOLDER_PATH = "Localization/pt-br/";
    private const string OTHER_FOLDER_PATH = "Other/";

    // Files
    private const string CREDITS_PATH = "Credits";
    private const string UI_LABELS_PATH = "UILabels";

    //--------------------------------------------------------------------------------//
    // GETTERS

    // Folders
    public static string GetFilesFolderPath () { return FILES_FOLDER_PATH; }
    public static string GetLevelsThumbnailsPath () { return LEVELS_THUMBNAILS_PATH; }
    public static string GetLocalizationEnglishFolderPath () { return LOCALIZATION_ENGLISH_FOLDER_PATH; }
    public static string GetLocalizationPortugueseFolderPath () { return LOCALIZATION_PORTUGUESE_FOLDER_PATH; }
    public static string GetOtherFolderPath () { return OTHER_FOLDER_PATH; }

    // Files
    public static string GetCreditsPath () { return CREDITS_PATH; }
    public static string GetUILabelsPath () { return UI_LABELS_PATH; }

    //--------------------------------------------------------------------------------//

    // Loads an file
    public static string LoadAsset (string folderName, string filePath)
    {
        // Config
        string newPath = (string.IsNullOrEmpty (folderName) ? filePath : string.Concat (folderName, filePath));
        newPath = string.Concat ("Files/", newPath);
        TextAsset textAsset = Resources.Load (newPath) as TextAsset;
        string content = (textAsset ? textAsset.text : string.Empty);
        return content;
    }
}