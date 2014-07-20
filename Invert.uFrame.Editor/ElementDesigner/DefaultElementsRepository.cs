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

    IElementDesignerData Data { get; set; }

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

    string GetEditableViewFilename(ViewData nameAsView);
    string GetEditableViewComponentFilename(ViewComponentData name);
    string GetEditableSceneManagerFilename(SceneManagerData nameAsSceneManager);
    string GetEditableSceneManagerSettingsFilename(SceneManagerData nameAsSettings);
    string GetEditableControllerFilename(ElementData controllerName);
    string GetEditableViewModelFilename(ElementData nameAsViewModel);
    string GetEnumsFilename(EnumData name);
    
    void MoveTo(ICodePathStrategy strategy,string name,ElementsDesigner designerWindow);
}

public class DefaultCodePathStrategy : ICodePathStrategy
{
    public IElementDesignerData Data { get; set; }

    public string AssetPath { get; set; }

    public virtual string BehavioursPath
    {
        get { return Path.Combine(AssetPath, "Behaviours"); }
    }
    public virtual string ScenesPath
    {
        get { return Path.Combine(AssetPath, "Scenes"); }
    }

    public virtual string GetControllersFileName(string name)
    {
        return name + "Controllers.designer.cs";
    }

    public virtual string GetViewsFileName(string name)
    {
        return name + "Views.designer.cs";
    }

    public virtual string GetViewModelsFileName(string name)
    {
        return name + ".designer.cs";
    }

    public virtual string GetEditableViewFilename(ViewData nameAsView)
    {
        return Path.Combine("Views", nameAsView.NameAsView + ".cs");
    }

    public virtual string GetEditableViewComponentFilename(ViewComponentData name)
    {
        return Path.Combine("ViewComponents", name.Name + ".cs");
    }

    public virtual string GetEditableSceneManagerFilename(SceneManagerData nameAsSceneManager)
    {
        return Path.Combine("SceneManagers", nameAsSceneManager.NameAsSceneManager + ".cs");
    }

    public virtual string GetEditableSceneManagerSettingsFilename(SceneManagerData nameAsSettings)
    {
        return Path.Combine("SceneManagers", nameAsSettings.NameAsSettings + ".cs");
    }

    public virtual string GetEditableControllerFilename(ElementData controllerName)
    {
        return Path.Combine("Controllers", controllerName.NameAsController + ".cs");
    }

    public virtual string GetEditableViewModelFilename(ElementData nameAsViewModel)
    {
        return Path.Combine("ViewModels", nameAsViewModel.NameAsViewModel + ".cs");
    }

    public virtual string GetEnumsFilename(EnumData name)
    {
        return GetViewModelsFileName(name.Data.Name);
    }

    public virtual void MoveTo(ICodePathStrategy strategy, string name, ElementsDesigner designerWindow)
    {
        var sourceFiles = uFrameEditor.GetAllFileGenerators(Data, this).ToArray();
        strategy.Data = Data;
        strategy.AssetPath = AssetPath;
        var targetFiles = uFrameEditor.GetAllFileGenerators(Data, strategy).ToArray();

        if (sourceFiles.Length == targetFiles.Length)
        {
            // Attempt to move every file
            ProcessMove(strategy, name, sourceFiles, targetFiles);
        }
        else
        {
            // Attempt to move non designer files
           // var designerFiles = sourceFiles.Where(p => p.Filename.EndsWith("designer.cs"));
            sourceFiles = sourceFiles.Where(p => !p.Filename.EndsWith("designer.cs")).ToArray();
            targetFiles = targetFiles.Where(p => !p.Filename.EndsWith("designer.cs")).ToArray();
            if (sourceFiles.Length == targetFiles.Length)
            {
                ProcessMove(strategy,name,sourceFiles,targetFiles);
                //// Remove all designer files
                //foreach (var designerFile in designerFiles)
                //{
                //    File.Delete(System.IO.Path.Combine(AssetPath, designerFile.Filename));
                //}
                //var saveCommand = uFrameEditor.Container.Resolve<IToolbarCommand>("SaveCommand");
                //saveCommand.Execute();
            }
        }
        
    }

    protected virtual void ProcessMove(ICodePathStrategy strategy, string name, CodeFileGenerator[] sourceFiles,
        CodeFileGenerator[] targetFiles)
    {
        for (int index = 0; index < sourceFiles.Length; index++)
        {
            var sourceFile = sourceFiles[index];
            var targetFile = targetFiles[index];

            var sourceFileInfo = new FileInfo(System.IO.Path.Combine(AssetPath, sourceFile.Filename));
            var targetFileInfo = new FileInfo(System.IO.Path.Combine(AssetPath, targetFile.Filename));
            if (sourceFileInfo.FullName == targetFileInfo.FullName) continue;
            if (!sourceFileInfo.Exists) continue;
            EnsurePath(sourceFileInfo);
            if (targetFileInfo.Exists) continue;
            EnsurePath(targetFileInfo);
            
            //sourceFileInfo.MoveTo(targetFileInfo.FullName);
            Debug.Log(Application.dataPath);
            var sourceAsset = "Assets" + sourceFileInfo.FullName.Replace("\\", "/").Replace(Application.dataPath, "").Replace("\\", "/");
            var targetAsset = "Assets" + targetFileInfo.FullName.Replace("\\", "/").Replace(Application.dataPath, "").Replace("\\", "/");
            Debug.Log(string.Format("Moving file {0} to {1}",sourceAsset, targetAsset));
            AssetDatabase.MoveAsset(sourceAsset, targetAsset);
        }
 
        Data.Settings.CodePathStrategy = strategy;
        Data.Settings.CodePathStrategyName = name;

        EditorUtility.SetDirty(Data as UnityEngine.Object);
        AssetDatabase.SaveAssets();
        EditorApplication.SaveAssets();
        AssetDatabase.Refresh();
        ////Clean up old directories
        //foreach (var sourceFile in sourceFiles)
        //{
        //    var sourceFileInfo = new FileInfo(System.IO.Path.Combine(AssetPath, sourceFile.Filename));
        //    if (sourceFileInfo.Directory != null)
        //    {
        //        if (!sourceFileInfo.Directory.Exists) continue;

        //        var directories = sourceFileInfo.Directory.GetDirectories("*", SearchOption.AllDirectories);
        //        foreach (var directory in directories)
        //        {
        //            if (directory.GetFiles("*").Count(x => x.Extension != ".meta" && x.Extension != "meta") == 0)
        //            {
        //                directory.Delete(true);
        //                Debug.Log("Removed Directory " + directory.FullName);
        //            }
        //        }
        //    }
        //}
        //AssetDatabase.Refresh();
    }

    protected void EnsurePath(FileInfo fileInfo)
    {
        
// Get the path to the directory
        var directory = System.IO.Path.GetDirectoryName(fileInfo.FullName);
        // Create it if it doesn't exist
        if (directory != null && !Directory.Exists(directory))
        {
            
            Directory.CreateDirectory(directory);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
    }
}

public class SubSystemPathStrategy : DefaultCodePathStrategy
{
    public IDiagramFilter FindFilter(IDiagramNode node)
    {
        var allFilters = Data.GetFilters();
        foreach (var diagramFilter in allFilters)
        {
            if (node != diagramFilter &&  diagramFilter.Locations.Keys.Contains(node.Identifier))
            {
                return diagramFilter;
            }
        }
        return null;
    }

    public SubSystemData FindSubsystem(IDiagramNode node)
    {
        var filter = FindFilter(node);
        if (filter == node) return null;
        if (filter == null) return null;

        while (!(filter is SubSystemData))
        {
            // Convert to node
            var filterNode = filter as IDiagramNode;
            
            // If its not a node at this point it must be hidden
            if (filterNode == null) return null;
            // Try again with the new filternode
            filter = FindFilter(filterNode);
            // if its null return
            if (filter == null)
            {
                return null;
            }
        }
        return filter as SubSystemData;
    }

    public string GetSubSystemPath(IDiagramNode node)
    {
        var subsystem = FindSubsystem(node);
        if (subsystem == null) return string.Empty;
        return subsystem.Name;
    }

    public override string GetEditableControllerFilename(ElementData controllerName)
    {
        return Path.Combine(GetSubSystemPath(controllerName), base.GetEditableControllerFilename(controllerName));
    }

    //public override string GetEditableSceneManagerFilename(SceneManagerData nameAsSceneManager)
    //{
    //    return Path.Combine(GetSubSystemPath(nameAsSceneManager),base.GetEditableSceneManagerFilename(nameAsSceneManager);
    //}

    public override string GetEditableViewFilename(ViewData nameAsView)
    {
        return Path.Combine(GetSubSystemPath(nameAsView),base.GetEditableViewFilename(nameAsView));
    }

    public override string GetEditableViewModelFilename(ElementData nameAsViewModel)
    {
        return Path.Combine(GetSubSystemPath(nameAsViewModel),base.GetEditableViewModelFilename(nameAsViewModel));
    }

    public override string GetEditableViewComponentFilename(ViewComponentData name)
    {
        return Path.Combine(GetSubSystemPath(name),base.GetEditableViewComponentFilename(name));
    }
    
}