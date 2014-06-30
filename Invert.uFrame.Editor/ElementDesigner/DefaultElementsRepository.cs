using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.Refactoring;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class DefaultElementsRepository : IElementsDataRepository
{
    //#region Paths

    //public string AssetPath
    //{
    //    get
    //    {
    //        return AssetDatabase.GetAssetPath(Diagram).Replace(string.Format("{0}.asset", Diagram.name), "");
    //    }
    //}

    //public string BehavioursPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "Behaviours");
    //    }
    //}

    //public string CreatePath(string name)
    //{
    //    var path = Path.Combine(AssetPath, "Behaviours");
    //    if (!Directory.Exists(path))
    //    {
    //        Directory.CreateDirectory(path);
    //        AssetDatabase.Refresh();
    //    }
    //    return path;
    //}
    //public string ControllersDesignerFileFullPath
    //{
    //    get
    //    {
    //        return Path.Combine(RootPath, ControllersDesignerFilename);
    //    }
    //}

    //public string ControllersDesignerFilename
    //{
    //    get
    //    {
    //        return AssetPath + string.Format("{0}Controllers.designer.cs", Diagram.Name);
    //    }
    //}

    //public string ControllersPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "Controllers");
    //    }
    //}

    //public FileInfo DiagramFileInfo
    //{
    //    get
    //    {
    //        return new FileInfo(Path.Combine(RootPath, DiagramPath));
    //    }
    //}

    //public string DiagramPath
    //{
    //    get { return AssetDatabase.GetAssetPath(Diagram); }
    //}

    //public string RootPath
    //{
    //    get
    //    {
    //        return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
    //    }
    //}

    //public string SceneManagersPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "SceneManagers");
    //    }
    //}

    //public string ScenesPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "Scenes");
    //    }
    //}

    //public string ViewComponentsPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "Components");
    //    }
    //}

    //public string ViewModelsDesignerFileFullPath
    //{
    //    get
    //    {
    //        return Path.Combine(RootPath, ViewModelsDesignerFilename);
    //    }
    //}

    //public string ViewModelsDesignerFilename
    //{
    //    get
    //    {
    //        return AssetPath + string.Format("{0}.designer.cs", Diagram.Name);
    //    }
    //}

    //public string ViewModelsPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "ViewModels");
    //    }
    //}

    //public string ViewsDesignerFileFullPath
    //{
    //    get
    //    {
    //        return Path.Combine(RootPath, ViewsDesignerFilename);
    //    }
    //}

    //public string ViewsDesignerFilename
    //{
    //    get
    //    {
    //        return AssetPath + string.Format("{0}Views.designer.cs", Diagram.Name);
    //    }
    //}

    //public string ViewsPath
    //{
    //    get
    //    {
    //        return Path.Combine(AssetPath, "Views");
    //    }
    //}



    //public ElementDesignerData GetData()
    //{
    //    //Diagram.ImportedElements.RemoveAll(p => Type.GetType(p.TypeAssemblyName) == null);
    //    //foreach (var importedElementData in Diagram.ImportedElements)
    //    //{
    //    //    importedElementData.Properties.Clear();
    //    //    importedElementData.Collections.Clear();
    //    //    importedElementData.Commands.Clear();
    //    //    var type = Type.GetType(importedElementData.TypeAssemblyName);
    //    //    FillElementData(type, importedElementData);
    //    //}

    //    return Diagram;
    //}

    //#endregion Paths

    [Inject]
    public IUFrameContainer Container { get; set; }

    private Dictionary<string, string> _derivedTypes;

    public void MarkDirty(IElementDesignerData data)
    {
        EditorUtility.SetDirty(data as UnityEngine.Object);
    }

    private UnityEngine.Object[] _Assets;

    public void RecacheAssets()
    {
        
    }
    public UnityEngine.Object[] GetAssets()
    {
        var tempObjects = new List<UnityEngine.Object>();
        var directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*" + ".asset", SearchOption.AllDirectories);

        int i = 0; int goFileInfoLength = goFileInfo.Length;
        for (; i < goFileInfoLength; i++)
        {
            FileInfo tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
                continue;

            string tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            try
            {
                
                var tempGo = AssetDatabase.LoadAssetAtPath(tempFilePath, RepositoryFor) as UnityEngine.Object;
                if (tempGo == null)
                {
                    
                }
                else
                {
                    tempObjects.Add(tempGo);
                    continue;
                }
            }
            catch (Exception ex)
            {
                continue;
            }

        }

        return tempObjects.ToArray();
    }

    public Dictionary<string, string> GetProjectDiagrams()
    {
        var items = new Dictionary<string, string>();
        foreach (var elementDesignerData in UFrameAssetManager.Diagrams.Where(p=>p.GetType() == RepositoryFor))
        {
            items.Add(elementDesignerData.Name, AssetDatabase.GetAssetPath(elementDesignerData as UnityEngine.Object));
        }
        return items;
    }

    public virtual void CreateNewDiagram()
    {
        UFrameAssetManager.CreateAsset<ElementDesignerData>();
    }

    public virtual Type RepositoryFor
    {
        get { return typeof (ElementDesignerData); }
    }

    public IElementDesignerData LoadDiagram(string path)
    {
        var data = AssetDatabase.LoadAssetAtPath(path, RepositoryFor) as IElementDesignerData;
        if (data == null)
        {
            return null;
        }

        return data;
    }

    public void SaveDiagram(IElementDesignerData data)
    {
        EditorUtility.SetDirty(data as UnityEngine.Object);
        AssetDatabase.SaveAssets();
    }

    public void RecordUndo(IElementDesignerData data, string title)
    {
        Undo.RecordObject(data as UnityEngine.Object, title);
    }

    public void FastUpdate()
    {

    }

    public void FastSave()
    {

    }
}

public interface ICodePathStrategy
{
    /// <summary>
    /// The root path to the diagram file
    /// </summary>
    string AssetPath { get; set; }

    /// <summary>
    /// Where behaviours are stored
    /// </summary>
    string BehavioursPath { get; }

    /// <summary>
    /// Where scenes are stored
    /// </summary>
    string ScenesPath { get; }

    /// <summary>
    /// The relative path to the controller designer file
    /// </summary>
    string GetControllersFileName(string name);

    /// <summary>
    /// The relative path to the views designer file
    /// </summary>
    string GetViewsFileName(string name);

    /// <summary>
    /// The relative path to the view-models designer file
    /// </summary>
    string GetViewModelsFileName(string name);

    string GetEditableViewFilename(string nameAsView);
    string GetEditableViewComponentFilename(string name);
    string GetEditableSceneManagerFilename(string nameAsSceneManager);
    string GetEditableSceneManagerSettingsFilename(string nameAsSettings);
    string GetEditableControllerFilename(string controllerName);
    string GetEditableViewModelFilename(string nameAsViewModel);
    string GetEnumsFilename(string name);
}

public class DefaultCodePathStrategy : ICodePathStrategy
{
    public string AssetPath { get; set; }

    public string BehavioursPath
    {
        get { return Path.Combine(AssetPath, "Behaviours"); }
    }
    public string ScenesPath
    {
        get { return Path.Combine(AssetPath, "Scenes"); }
    }

    public string GetControllersFileName(string name)
    {
        return name + "Controllers.designer.cs";
    }

    public string GetViewsFileName(string name)
    {
        return name + "Views.designer.cs";
    }

    public string GetViewModelsFileName(string name)
    {
        return name + ".designer.cs";
    }

    public string GetEditableViewFilename(string nameAsView)
    {
        return Path.Combine("Views", nameAsView + ".cs");
    }

    public string GetEditableViewComponentFilename(string name)
    {
        return Path.Combine("ViewComponents", name + ".cs");
    }

    public string GetEditableSceneManagerFilename(string nameAsSceneManager)
    {
        return Path.Combine("SceneManagers", nameAsSceneManager + ".cs");
    }

    public string GetEditableSceneManagerSettingsFilename(string nameAsSettings)
    {
        return Path.Combine("SceneManagers", nameAsSettings + ".cs");
    }

    public string GetEditableControllerFilename(string controllerName)
    {
        return Path.Combine("Controllers", controllerName + ".cs");
    }

    public string GetEditableViewModelFilename(string nameAsViewModel)
    {
        return Path.Combine("ViewModels", nameAsViewModel + ".cs");
    }

    public string GetEnumsFilename(string name)
    {
        return GetViewModelsFileName(name);
    }
}