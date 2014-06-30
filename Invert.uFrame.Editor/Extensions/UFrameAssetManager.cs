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
#if DEBUG
    [MenuItem("Assets/[u]Frame/New Element Diagram (Legacy Asset)", false, 41)]
    public static void NewViewModelDiagram()
    {
        uFrameEditor.Container.Resolve<IElementsDataRepository>(".asset").CreateNewDiagram();
    }
#endif

    [MenuItem("Assets/[u]Frame/New Element Diagram", false, 40)]
    public static void NewJsonViewModelDiagram()
    {
        uFrameEditor.Container.Resolve<IElementsDataRepository>(".json").CreateNewDiagram();
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
        var repos = uFrameEditor.Container.ResolveAll<IElementsDataRepository>().OfType<DefaultElementsRepository>();

        Diagrams = repos.SelectMany(p => p.GetAssets()).OfType<IElementDesignerData>().ToList();
#if DEBUG
        Debug.Log(string.Join(Environment.NewLine, Diagrams.Select(p=>p.Name + ":" + p.Identifier).ToArray()));
#endif
        DiagramNames = Diagrams.Select(p => p.Name).ToArray();
    }
}