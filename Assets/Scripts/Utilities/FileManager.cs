using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class FileManager
    {
        public static string FilesFolderPath => "Files/";

        public static string LevelsThumbnailsPath => "Levels_Thumbnails";

        public static string LocalizationEnglishFolderPath => "Localization/en/";

        public static string LocalizationPortugueseFolderPath => "Localization/pt-br/";

        public static string OtherFolderPath => "Other/";

        public static string CreditsPath => "Credits";

        public static string UILabelsPath => "UILabels";

        // Loads an file
        public static string LoadAsset(string folderName, string filePath)
        {
            string newPath = (string.IsNullOrEmpty(folderName) ? filePath : string.Concat(folderName, filePath));
            newPath = string.Concat("Files/", newPath);
            TextAsset textAsset = Resources.Load(newPath) as TextAsset;
            string content = (textAsset ? textAsset.text : string.Empty);
            return content;
        }

        public static IEnumerator LoadAssetAsync(string folderName, string filePath, UnityAction<string> callback)
        {
            string newPath = (string.IsNullOrEmpty(folderName) ? filePath : string.Concat(folderName, filePath));
            newPath = string.Concat("Files/", newPath);
            ResourceRequest request = Resources.LoadAsync<TextAsset>(newPath);
            yield return request;
            yield return new WaitUntil(() => request.isDone);
            TextAsset textAsset = request.asset as TextAsset;
            callback(textAsset ? textAsset.text : string.Empty);
        }
    }
}