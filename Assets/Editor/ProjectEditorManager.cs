using MVC.Global;
using System;
using UnityEditor;
using UnityEditor.Build;
using Utilities;

public class ProjectEditorManager
{
    /// <summary>
    /// Get project info to update editor values
    /// </summary>
    [MenuItem("Project/Update Properties", false, 1)]
    protected static void UpdateProperties()
    {
        try
        {
            SerializedObject playerSettingsManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(Configuration.Properties.ProjectSettings)[0]);
            PlayerSettings.companyName = ProjectInfo.CompanyName;
            PlayerSettings.productName = ProjectInfo.ProductName;
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, ProjectInfo.Identifier);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, ProjectInfo.Identifier);
            SerializedProperty bundleVersion = playerSettingsManager.FindProperty("bundleVersion");
            SerializedProperty androidBundleVersionCode = playerSettingsManager.FindProperty("AndroidBundleVersionCode");
            SerializedProperty iOSTargetOSVersionString = playerSettingsManager.FindProperty("iOSTargetOSVersionString");
            SerializedProperty buildNumberiOS = playerSettingsManager.FindProperty("buildNumber");
            SerializedProperty iphoneBuild = buildNumberiOS.GetArrayElementAtIndex(0);
            iphoneBuild.NextVisible(true);
            iphoneBuild.NextVisible(true);

            iOSTargetOSVersionString.stringValue = ProjectInfo.iOSVersion;
            bundleVersion.stringValue = ProjectInfo.Version;
            androidBundleVersionCode.intValue = ProjectInfo.BundleVersion;
            iphoneBuild.stringValue = ProjectInfo.BundleVersion.ToString();

#if UNITY_ANDROID
            PlayerSettings.preserveFramebufferAlpha = false;
#elif UNITY_IOS
            PlayerSettings.preserveFramebufferAlpha = true;
#endif

            playerSettingsManager.ApplyModifiedProperties();
            playerSettingsManager.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
