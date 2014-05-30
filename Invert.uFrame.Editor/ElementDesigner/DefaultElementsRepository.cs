using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.Refactoring;
using UnityEditor;
using UnityEngine;

public abstract class DefaultElementsRepository : IElementsDataRepository
{ 
    #region Paths

    public string AssetPath
    {
        get
        {
            return AssetDatabase.GetAssetPath(Diagram).Replace(string.Format("{0}.asset", Diagram.name), "");
        }
    }

    public string BehavioursPath
    {
        get
        {
            return Path.Combine(AssetPath, "Behaviours");
        }
    }

    public string CreatePath(string name)
    {
        var path = Path.Combine(AssetPath, "Behaviours");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
        return path;
    }
    public string ControllersDesignerFileFullPath
    {
        get
        {
            return Path.Combine(RootPath, ControllersDesignerFilename);
        }
    }

    public string ControllersDesignerFilename
    {
        get
        {
            return AssetPath + string.Format("{0}Controllers.designer.cs", Diagram.Name);
        }
    }

    public string ControllersPath
    {
        get
        {
            return Path.Combine(AssetPath, "Controllers");
        }
    }

    public FileInfo DiagramFileInfo
    {
        get
        {
            return new FileInfo(Path.Combine(RootPath, DiagramPath));
        }
    }

    public string DiagramPath
    {
        get { return AssetDatabase.GetAssetPath(Diagram); }
    }

    public string RootPath
    {
        get
        {
            return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length);
        }
    }

    public string SceneManagersPath
    {
        get
        {
            return Path.Combine(AssetPath, "SceneManagers");
        }
    }

    public string ScenesPath
    {
        get
        {
            return Path.Combine(AssetPath, "Scenes");
        }
    }

    public string ViewComponentsPath
    {
        get
        {
            return Path.Combine(AssetPath, "Components");
        }
    }

    public string ViewModelsDesignerFileFullPath
    {
        get
        {
            return Path.Combine(RootPath, ViewModelsDesignerFilename);
        }
    }

    public string ViewModelsDesignerFilename
    {
        get
        {
            return AssetPath + string.Format("{0}.designer.cs", Diagram.Name);
        }
    }

    public string ViewModelsPath
    {
        get
        {
            return Path.Combine(AssetPath, "ViewModels");
        }
    }

    public string ViewsDesignerFileFullPath
    {
        get
        {
            return Path.Combine(RootPath, ViewsDesignerFilename);
        }
    }

    public string ViewsDesignerFilename
    {
        get
        {
            return AssetPath + string.Format("{0}Views.designer.cs", Diagram.Name);
        }
    }

    public string ViewsPath
    {
        get
        {
            return Path.Combine(AssetPath, "Views");
        }
    }

    public string GetContainerCustomFileFullPath(string name)
    {
        return Path.Combine(RootPath, GetContainerCustomFilename(name)).Replace("\\", "/");
    }

    public string GetContainerCustomFilename(string name)
    {
        return Path.Combine(SceneManagersPath,
            string.Format("{0}.cs", name)).Replace("\\", "/");
    }

    public string GetControllerCustomFileFullPath(string name)
    {
        return Path.Combine(RootPath, GetControllerCustomFilename(name)).Replace("\\", "/");
    }

    public string GetControllerCustomFilename(string name)
    {
        return Path.Combine(ControllersPath, string.Format("{0}Controller.cs", name.Replace("ViewModel", ""))).Replace("\\", "/");
    }

    public string GetViewComponentCustomFileFullPath(string name)
    {
        return Path.Combine(RootPath, GetViewComponentCustomFilename(name)).Replace("\\", "/");
    }

    public string GetViewComponentCustomFilename(string name)
    {
        return Path.Combine(ViewComponentsPath, string.Format("{0}.cs", name)).Replace("\\", "/");
    }

    public string GetViewCustomFileFullPath(string name)
    {
        return Path.Combine(RootPath, GetViewCustomFilename(name)).Replace("\\", "/");
    }

    public string GetViewCustomFilename(string name)
    {
        return Path.Combine(ViewsPath, string.Format("{0}.cs", name)).Replace("\\", "/");
    }

    public string GetViewModelCustomFileFullPath(string name)
    {
        return Path.Combine(RootPath, GetViewModelCustomFilename(name)).Replace("\\", "/");
    }

    public string GetViewModelCustomFilename(string name)
    {
        return Path.Combine(ViewModelsPath, string.Format("{0}ViewModel.cs", name)).Replace("\\", "/");
    }

    public ElementDesignerData GetData()
    {
        Diagram.ImportedElements.RemoveAll(p => Type.GetType(p.TypeAssemblyName) == null);
        foreach (var importedElementData in Diagram.ImportedElements)
        {
            importedElementData.Properties.Clear();
            importedElementData.Collections.Clear();
            importedElementData.Commands.Clear();
            var type = Type.GetType(importedElementData.TypeAssemblyName);
            FillElementData(type, importedElementData);
        }

        return Diagram;
    }

    protected abstract void FillElementData(Type type, ElementDataBase importedElementData);

    public abstract void Save();
    public abstract void CreateScene(SceneManagerData sceneManagerData);

    #endregion Paths

    private Dictionary<string, string> _derivedTypes;
    private SerializedObject _serializedObject;    
    public ElementDesignerData Diagram { get; set; }

    public IEnumerable<string> GetAllFiles(bool fullPaths = true)
    {
        var files = DiagramFileInfo.Directory.GetFiles("*.cs", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (file.FullName.EndsWith(".designer.cs")) continue;
            yield return file.FullName;
        }
    }
    public SerializedObject SerializedObject
    {
        get { return _serializedObject ?? (_serializedObject = new SerializedObject(Diagram)); }
        set { _serializedObject = value; }
    }
    public IEnumerable<string> GetCustomFilePaths(IDiagramItem item, bool fullPaths, bool includeRefactors = false)
    {
        var sceneManager = item as SceneManagerData;
        if (sceneManager != null)
        {
            yield return fullPaths ? GetContainerCustomFileFullPath(sceneManager.Name) : GetContainerCustomFilename(sceneManager.Name);
            if (includeRefactors)
            {
                yield return fullPaths ? GetContainerCustomFileFullPath(sceneManager.OldName) : GetContainerCustomFilename(sceneManager.OldName);
            }
        }
        var element = item as ElementData;
        if (element != null)
        {
            yield return fullPaths ? GetViewModelCustomFileFullPath(element.Name) : GetViewModelCustomFilename(element.Name);
            yield return fullPaths ? GetControllerCustomFileFullPath(element.Name) : GetControllerCustomFilename(element.Name);
            if (includeRefactors)
            {
                yield return fullPaths ? GetViewModelCustomFileFullPath(element.OldName) : GetViewModelCustomFilename(element.OldName);
                yield return fullPaths ? GetControllerCustomFileFullPath(element.OldName) : GetControllerCustomFilename(element.OldName);
            }
            //yield return fullPaths ? GetViewCustomFileFullPath(element) : GetContainerCustomFilename(element);
        }
        var view = item as ViewData;
        if (view != null)
        {
            yield return fullPaths ?
                GetViewCustomFileFullPath(view.Name) :
                GetViewCustomFilename(view.Name);
            if (includeRefactors)
            {
                yield return fullPaths ? GetViewCustomFileFullPath(view.OldName) : GetViewCustomFilename(view.OldName);
            }
        }
        var viewComponent = item as ViewComponentData;
        if (viewComponent != null)
        {
            yield return fullPaths ?
                GetViewComponentCustomFileFullPath(viewComponent.Name) :
                GetViewComponentCustomFilename(viewComponent.Name);
            if (includeRefactors)
            {
                yield return fullPaths ? GetViewComponentCustomFileFullPath(viewComponent.OldName) : GetViewComponentCustomFilename(viewComponent.OldName);
            }
        }
    }
    public virtual IEnumerable<ElementItemType> GetAvailableTypes(bool allowNone, bool primitiveOnly = false)
    {
        if (allowNone)
        {
            yield return new ElementItemType() { AssemblyQualifiedName = null, Group = "", Label = "None" };
        }

        if (!primitiveOnly)
        {
            foreach (var viewModel in Diagram.ViewModels)
            {
                yield return new ElementItemType()
                {
                    AssemblyQualifiedName = viewModel.AssemblyQualifiedName,
                    Label = viewModel.Name,
                    Group = Diagram.name
                };
            }
        }
        yield return new ElementItemType() { Type = typeof(int), Group = "", Label = "int" };
        yield return new ElementItemType() { Type = typeof(string), Group = "", Label = "string" };
        yield return new ElementItemType() { Type = typeof(decimal), Group = "", Label = "decimal" };
        yield return new ElementItemType() { Type = typeof(float), Group = "", Label = "float" };
        yield return new ElementItemType() { Type = typeof(bool), Group = "", Label = "bool" };
        yield return new ElementItemType() { Type = typeof(char), Group = "", Label = "char" };
        yield return new ElementItemType() { Type = typeof(DateTime), Group = "", Label = "date" };
        yield return new ElementItemType() { Type = typeof(Vector2), Group = "", Label = "Vector2" };
        yield return new ElementItemType() { Type = typeof(Vector3), Group = "", Label = "Vector3" };

        if (primitiveOnly) yield break;
       
    }
    public static IEnumerable<Type> GetDerivedTypes<T>(bool includeAbstract = false, bool includeBase = true)
    {
        var type = typeof(T);
        if (includeBase)
            yield return type;
        if (includeAbstract)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(type)))
                {
                    yield return t;
                }
            }
        }
        else
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(type) && !x.IsAbstract))
                {
                    yield return t;
                }
            }
        }
    }
  
    public bool IsImportOnly(Type type)
    {
        if (type == null) return false;
        var attribute =
            type.GetCustomAttributes(false).FirstOrDefault();
        if (attribute != null && attribute.GetType().Name == "DiagramInfoAttribute")
        {
            return true;
        }
        if (attribute == null)
            return false;

        return true;
    }

    
    public abstract DiagramItem ImportType(Type item);

    /// <summary>
    /// Should navigate to a view in the scene based on the type.
    /// </summary>
    /// <param name="data">The View Data</param>
    public abstract void NavigateToView(ViewData data);

    /// <summary>
    /// Execute all the refactorings queued in the diagram
    /// </summary>
    public void ProcessRefactorings()
    {
        var refactorer = new RefactorContext(Diagram.Refactorings);
        var files = GetAllFiles(true).ToArray();
        if (refactorer.Refactors.Count > 0)
        {
            refactorer.Refactor(files);
        }
        Debug.Log(string.Format("Applied {0} refactors.", refactorer.Refactors.Count));
        Diagram.Applied();
    }

}

