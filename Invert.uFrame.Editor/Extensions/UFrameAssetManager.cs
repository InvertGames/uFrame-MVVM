using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.uFrame.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public class UFrameAssetManager : AssetPostprocessor
{
    public const string VM_ASSEMBLY_NAME = "ViewModel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
    [MenuItem("Assets/[u]Frame/New Element Diagram", false, 40)]
    public static void NewViewModelDiagram()
    {
        uFrameEditor.Container.Resolve<IElementsDataRepository>().CreateNewDiagram();
        
    }

    private static List<IElementDesignerData> _diagrams;

    public static IAssemblyNameProvider AssemblyNameProvider { get; set; }
    private static string _designerVMAssemblyName = null;
    private static string[] _diagramNames;

    public static string DesignerVMAssemblyName
    {
        get
        {
            return VM_ASSEMBLY_NAME;
            //if (AssemblyNameProvider == null)
            //{
            //    return VM_ASSEMBLY_NAME;
            //}
            //return _designerVMAssemblyName ?? (_designerVMAssemblyName = AssemblyNameProvider.AssemblyName.Replace("AssemblyNameProvider", "ViewModel"));
        }
    }

    public static string[] DiagramNames
    {
        get
        {
            if (_diagramNames == null)
            {
                Refresh();
            }
            return _diagramNames;
        }
        set { _diagramNames = value; }
    }

    public static List<IElementDesignerData> Diagrams
    {
        get
        {
            if (_diagrams == null)
            {
                Refresh();
            }
            return _diagrams;
        }
        set { _diagrams = value; }
    }


    static UFrameAssetManager()
    {
        Refresh();
    }

    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static T CreateAsset<T>(string assetPath = null, string assetName = null) where T : ScriptableObject
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

        string assetPathAndName = assetName == null ? AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset") : path + "/" + assetName + ".asset";

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
    public static Object[] GetAssetsOfType(Type type, string fileExtension)
    {
        var tempObjects = new List<Object>();
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

            tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            try
            {
               
            tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(ElementDesignerData)) as ElementDesignerData;
            if (tempGO == null)
            {
            }
            else
            {
                tempObjects.Add(tempGO);
                continue;
            }

            }
            catch (Exception ex)
            {
                continue;
            }  
            
            //tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UBGlobals)) as UBGlobals;
            //if (tempGO == null)
            //{
            //    continue;
            //}
            //else
            //{
            //    tempObjects.Add(tempGO);
            //}
        }

        return tempObjects.ToArray();
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
            yes = str.EndsWith(".asset");
            var name = Path.GetFileNameWithoutExtension(str);
            if (DiagramNames.Any(p => p.ToUpper() == name.ToUpper()))
            {
                yes = false;
            }


        }


        if (!yes)
            foreach (var str in deletedAssets)
                if (str.EndsWith(".asset"))
                    yes = true;
        //if (!yes)
        //    for (var i = 0; i < movedAssets.Length; i++)
        //        if (movedAssets[i].EndsWith(".asset") || movedAssets[i].EndsWith(".prefab"))
        //            yes = true;

        if (yes)
        {
            Refresh();
        }
    }



    private static void Refresh()
    {

        Diagrams = GetAssetsOfType(typeof(ElementDesignerData), ".asset").Cast<IElementDesignerData>().ToList();
        DiagramNames = Diagrams.Select(p => p.Name).ToArray();


    }
}