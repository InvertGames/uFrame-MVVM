using Invert.uFrame.Editor;
using UnityEditor;

public class uFrameVersionProcessor : AssetPostprocessor
{
    private const string CURRENT_VERSION = "1.283";
    private const string VERSION_KEY = "uFrame.InstalledVersion";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        var lastVersion = EditorPrefs.GetString(VERSION_KEY, "");
        if (lastVersion != CURRENT_VERSION)
        {
            EditorApplication.delayCall += ShowChangeLog;
            EditorPrefs.SetString(VERSION_KEY, CURRENT_VERSION);
        }
    }

    private static void ShowChangeLog()
    {
        EditorApplication.delayCall -= ShowChangeLog;

        uFrameStartDialog.ShowWindow();
    }
}