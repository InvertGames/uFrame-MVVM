using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public class UBAssetManager : AssetPostprocessor
{
    private static List<UBSharedBehaviour> _behaviours = new List<UBSharedBehaviour>();
    private static List<Object> _behaviourPrefabs = new List<Object>();
    private static List<UBGlobals> _globals;

    public static string[] BehaviourNames
    {
        get;
        set;
    }

    public static List<UBSharedBehaviour> Behaviours
    {
        get { return _behaviours; }
        set { _behaviours = value; }
    }
    public static List<UnityEngine.Object> BehaviourPrefabs
    {
        get { return _behaviourPrefabs; }
        set { _behaviourPrefabs = value; }
    }
    public static List<UBGlobals> Globals
    {
        get { return _globals ?? (_globals = new List<UBGlobals>()); }
        set { _globals = value; }
    }

    static UBAssetManager()
    {
        RefreshBehaviours();
    }

    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static T CreateAsset<T>(string assetPath = null,string assetName = null) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = assetPath ?? AssetDatabase.GetAssetPath(Selection.activeObject);
        
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = assetName == null ? AssetDatabase.GenerateUniqueAssetPath(path + Path.DirectorySeparatorChar + "New " + typeof(T).ToString() + ".asset") : AssetDatabase.GenerateUniqueAssetPath(path + Path.DirectorySeparatorChar + assetName + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        return asset;
    }

    /// <summary>
    /// Used to get assets of a certain type and file extension from entire project
    /// </summary>
    /// <param name="type">The type to retrieve. eg typeof(GameObject).</param>
    /// <param name="fileExtension">The file extention the type uses eg ".prefab".</param>
    /// <returns>An Object array of assets.</returns>
    public static Object[] GetAssetsOfType(System.Type type, string fileExtension)
    {
        var tempObjects = new List<UnityEngine.Object>();
        var directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

        int i = 0; int goFileInfoLength = goFileInfo.Length;
        FileInfo tempGoFileInfo; string tempFilePath;
        Object tempGO;
        for (; i < goFileInfoLength; i++)
        {
            tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
                continue;
            var dataPath = Application.dataPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            
            tempFilePath = tempGoFileInfo.FullName.Replace(dataPath, "Assets");
            
            //tempFilePath = tempFilePath.Replace(Application.dataPath, "Assets");

            try
            {
                tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof (UBSharedBehaviour)) as UBSharedBehaviour;
                if (tempGO == null)
                {
                }

                else
                {
                    tempObjects.Add(tempGO);
                    continue;
                }

                tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof (UBGlobals)) as UBGlobals;
                if (tempGO == null)
                {
                    continue;
                }
                else
                {
                    tempObjects.Add(tempGO);
                }
            }
            catch (Exception ex)
            {
                // Suppress any unwanted messages from corrupt assets. Its not the time and place for them
            }
        }

        return tempObjects.ToArray();
    }

    [MenuItem("Assets/uBehaviours/Shared Behaviour", false, 40)]
    public static void NewUBehaviour()
    {
       CreateAsset<UBSharedBehaviour>();
    }

    public static UBSharedBehaviour NewUBehaviour(string name)
    {
        var asset = CreateAsset<UBSharedBehaviour>();
        if (name != null)
        {
            asset.name = name;
        }
        return asset;
    }

    [MenuItem("Assets/Create/Globals", false, 41)]
    public static void NewUBGlobals()
    {
        CreateAsset<UBGlobals>();
    }

    public static void OnPostprocessAllAssets(
        String[] importedAssets,
        String[] deletedAssets,
        String[] movedAssets,
        String[] movedFromAssetPaths)
    {
        var yes = false;
        foreach (var str in importedAssets)
        {
            yes = str.EndsWith(".asset") || str.EndsWith(".prefab");
            var name = Path.GetFileNameWithoutExtension(str);
            if (BehaviourPrefabs.Any(p => p.name == name))
            {
                yes = false;
            }
        }
            

        if (!yes)
            foreach (var str in deletedAssets)
                if (str.EndsWith(".asset") || str.EndsWith(".prefab"))
                    yes = true;
        //if (!yes)
        //    for (var i = 0; i < movedAssets.Length; i++)
        //        if (movedAssets[i].EndsWith(".asset") || movedAssets[i].EndsWith(".prefab"))
        //            yes = true;

        if (yes)
        {
            RefreshBehaviours();
            //EditorWindow.GetWindow<UBExplorerWindow>().Repaint();
        }
    }

    public override int GetPostprocessOrder()
    {
        return base.GetPostprocessOrder();
    }

    public static bool DebugMode
    {
        get { return EditorPrefs.GetBool("UBDebugMode", true); }
        set {EditorPrefs.SetBool("UBDebugMode",value); RefreshBehaviours();}
    }

    public static void RefreshBehaviours()
    {
        var list = GetAssetsOfType(typeof(UBSharedBehaviour), ".asset");
        //BehaviourPrefabs = GetAssetsOfType(typeof(UBInstanceBehaviour), ".prefab").ToList();
        
        Behaviours = list.OfType<UBSharedBehaviour>().ToList();
        BehaviourNames = Behaviours.Select(p => p.name).ToArray();
        Globals = list.OfType<UBGlobals>().ToList();
        //        Debug.Log("Loaded " + Behaviours.Count + " behaviours");
        //        Debug.Log("Loaded " + Globals.Count + " globals");
        
        
        
        UBDrawers.Drawers = null;
        if (DebugMode)
        {
            UBActionSheet.ExecutionHandler = new DebugExecutionHandler();
        }
        else
        {
            UBActionSheet.ExecutionHandler = new DefaultExecutionHandler();
        }
        
        if (PlaymodeStateChanged != EditorApplication.playmodeStateChanged)
            EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
    }

    private static void PlaymodeStateChanged()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            var debugHandler = UBActionSheet.ExecutionHandler as DebugExecutionHandler;
            if (debugHandler != null) debugHandler.Errors.Clear();
            if (debugHandler != null) debugHandler.Reset();
            
        }
    }
}