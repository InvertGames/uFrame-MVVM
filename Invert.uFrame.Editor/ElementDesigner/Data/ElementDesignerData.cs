using System.Security.Cryptography;
using Invert.uFrame.Editor.ElementDesigner.Data;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public interface IElementDesignerData
{
    // Settings
    ElementDiagramSettings Settings { get; }
    // Basic Information
    string Name { get; }
    string Version { get; set; }

    // Not Persisted
    int RefactorCount { get; set; }
    
   
  
    // Filters
    IDiagramFilter CurrentFilter { get; }
    DefaultFilter DefaultFilter { get;  }
    Stack<IDiagramFilter> FilterStack { get; }

    // Queries
    IEnumerable<IDiagramNode> AllDiagramItems { get; }
    IEnumerable<ElementDataBase> AllElements { get; }
    IEnumerable<IDiagramNode> AllowedDiagramItems { get; }
    IEnumerable<IDiagramNode> DiagramItems { get; }
    IEnumerable<ElementDataBase> Elements { get; }
    IEnumerable<IDiagramFilter> FilterPath { get; }
    IEnumerable<IDiagramFilter> Filters { get; }
    IEnumerable<IDiagramNode> ImportableItems { get; }

    // Filter Stuff
    List<Refactorer> Refactorings { get; }
    SceneFlowFilter SceneFlowFilter { get;  }

    // Node Data
    List<SceneManagerData> SceneManagers { get; }
    List<SubSystemData> SubSystems { get; set; }
    List<ViewComponentData> ViewComponents { get; }
    List<ElementData> ViewModels { get; }
    List<ViewData> Views { get; }
    List<ImportedElementData> ImportedElements { get; }
    List<EnumData> Enums { get; }

    // Dynamically loaded
    List<IDiagramLink> Links { get; }

    /// <summary>
    /// Should be called when first loaded.
    /// </summary>
    void Initialize();

    void FilterPushed(IDiagramFilter filter);
    void FilterPoped();
}

[Serializable]
public class ElementDiagramSettings
{
    [SerializeField]
    private Color _associationLinkColor = Color.white;
    [SerializeField]
    private Color _definitionLinkColor = Color.cyan; 
    [SerializeField]
    private Color _inheritanceLinkColor = Color.green;
    [SerializeField]
    private Color _sceneManagerLinkColor = Color.gray;
    [SerializeField]
    private Color _subSystemLinkColor = Color.grey;

    [SerializeField, HideInInspector]
    private string _codePathStrategyName = "Default";
    [SerializeField]
    private Color _transitionLinkColor = Color.yellow;
    [SerializeField]
    private Color _viewLinkColor = Color.blue;
    [SerializeField]
    private int _snapSize = 10;

    [NonSerialized]
    private ICodePathStrategy _codePathStrategy;

    public Color AssociationLinkColor
    {
        get { return _associationLinkColor; }
        set { _associationLinkColor = value; }
    }

    public Color DefinitionLinkColor
    {
        get { return _definitionLinkColor; }
        set { _definitionLinkColor = value; }
    }

    public Color InheritanceLinkColor
    {
        get { return _inheritanceLinkColor; }
        set { _inheritanceLinkColor = value; }
    }

    public Color SceneManagerLinkColor
    {
        get { return _sceneManagerLinkColor; }
        set { _sceneManagerLinkColor = value; }
    }

    public Color SubSystemLinkColor
    {
        get { return _subSystemLinkColor; }
        set { _subSystemLinkColor = value; }
    }

    public Color TransitionLinkColor
    {
        get { return _transitionLinkColor; }
        set { _transitionLinkColor = value; }
    }

    public Color ViewLinkColor
    {
        get { return _viewLinkColor; }
        set { _viewLinkColor = value; }
    }

    public int SnapSize
    {
        get { return _snapSize; }
        set { _snapSize = value; }
    }

    public string CodePathStrategyName
    {
        get { return string.IsNullOrEmpty(_codePathStrategyName) ? "Default" : _codePathStrategyName; }
        set { _codePathStrategyName = value; }
    }

    public ICodePathStrategy CodePathStrategy
    {
        get { return _codePathStrategy; }
        set { _codePathStrategy = value; }
    }
}

[SerializeField]
public class ElementDesignerData : ScriptableObject,  IElementDesignerData
{
    public ElementDiagramSettings Settings
    {
        get { return _settings ?? (_settings = new ElementDiagramSettings()); }
        set { _settings = value; }
    }

    [SerializeField, HideInInspector]
    private DefaultFilter _defaultFilter = new DefaultFilter();

 

    [SerializeField, HideInInspector]
    private List<EnumData> _enums = new List<EnumData>();

    [NonSerialized]
    private Stack<IDiagramFilter> _filterStack;

    [SerializeField, HideInInspector]
    private List<ImportedElementData> _importedElements = new List<ImportedElementData>();

   
    [NonSerialized]
    private List<IDiagramLink> _links = new List<IDiagramLink>();

    [SerializeField, HideInInspector]
    private List<string> _persistedFilterStack = new List<string>();

    [SerializeField, HideInInspector]
    private List<PluginData> _pluginItems = new List<PluginData>();

    [SerializeField, HideInInspector]
    private SceneFlowFilter _sceneFlowFilter = new SceneFlowFilter();

    

    [SerializeField, HideInInspector]
    private List<SceneManagerData> _SceneManagers = new List<SceneManagerData>();

 
    [SerializeField, HideInInspector]
    private List<SubSystemData> _subSystems = new List<SubSystemData>();


    [SerializeField, HideInInspector]
    private string _version;

    [SerializeField, HideInInspector]
    private List<ViewComponentData> _viewComponents = new List<ViewComponentData>();

  

    [SerializeField, HideInInspector]//, HideInInspector]
    private List<ElementData> _viewModels = new List<ElementData>();

    [SerializeField, HideInInspector]
    private List<ViewData> _views = new List<ViewData>();

    [SerializeField]
    private ElementDiagramSettings _settings;

    public IEnumerable<IDiagramNode> AllDiagramItems
    {
        get
        {
            return
                ViewModels.Cast<IDiagramNode>()
                    .Concat(ImportedElements.Cast<IDiagramNode>())
                    .Concat(Enums.Cast<IDiagramNode>())
                    .Concat(Views.Cast<IDiagramNode>())
                    .Concat(SceneManagers.Cast<IDiagramNode>())
                    .Concat(SubSystems.Cast<IDiagramNode>())
                    .Concat(ViewComponents.Cast<IDiagramNode>()
                );
        }
    }

    public IEnumerable<ElementDataBase> AllElements
    {
        get { return AllDiagramItems.OfType<ElementDataBase>(); }
    }

    public IEnumerable<IDiagramNode> AllowedDiagramItems
    {
        get { return AllDiagramItems.Where(p => CurrentFilter.IsAllowed(p, p.GetType())); }
    }

    public string AssetPath { get; set; }


    public ICodePathStrategy CodePathStrategy { get; set; }



    public string ControllersFileName
    {
        get
        {
            return name + "Controllers.designer.cs";
        }
    }

    public IDiagramFilter CurrentFilter
    {
        get
        {
            if (FilterStack.Count < 1)
            {
                return SceneFlowFilter;
            }
            return FilterStack.Peek();
        }
    }

    public DefaultFilter DefaultFilter
    {
        get { return _defaultFilter; }
        set { _defaultFilter = value; }
    }

    public IEnumerable<IDiagramNode> DiagramItems
    {
        get { return this.FilterItems(AllDiagramItems); }
    }

    public IEnumerable<ElementDataBase> Elements
    {
        get { return DiagramItems.OfType<ElementDataBase>(); }
    }

    public List<EnumData> Enums
    {
        get { return _enums; }
        set { _enums = value; }
    }

    public IEnumerable<IDiagramFilter> FilterPath
    {
        get { return FilterStack.Reverse(); }
    }

    public IEnumerable<IDiagramFilter> Filters
    {
        get { return AllDiagramItems.OfType<IDiagramFilter>(); }
    }

    //public bool GenerateSceneManager
    //{
    //    get { return _generateSceneManager; }
    //    set { _generateSceneManager = value; }
    //}
    public IEnumerable<IDiagramNode> ImportableItems
    {
        get
        {
            //return AllowedDiagramItems;
            return AllowedDiagramItems.Where(p => !CurrentFilter.Locations.Keys.Contains(p.Identifier)).ToArray();
            //return items.Where(p => !Filters.Any(x => x.Locations.Keys.Contains(p.Identifier)));
        }
    }

    public List<ImportedElementData> ImportedElements
    {
        get { return _importedElements; }
        set { _importedElements = value; }
    }



    public List<IDiagramLink> Links
    {
        get { return _links; }
        set { _links = value; }
    }

    public string Name
    {
        get
        {
            return Regex.Replace(name, "[^a-zA-Z0-9_.]+", "");
        }
    }

    public List<PluginData> PluginItems
    {
        get { return _pluginItems; }
        set { _pluginItems = value; }
    }

    public int RefactorCount { get; set; }

    public List<Refactorer> Refactorings
    {
        get
        {
            return
                AllDiagramItems.OfType<IRefactorable>()
                    .SelectMany(p => p.Refactorings)
                    .Concat(AllDiagramItems.SelectMany(p => p.Items).OfType<IRefactorable>().SelectMany(p => p.Refactorings))
                    .ToList();
        }
    }

    public SceneFlowFilter SceneFlowFilter
    {
        get
        {
            return _sceneFlowFilter;
        }
        set { _sceneFlowFilter = value; }
    }

    public List<SceneManagerData> SceneManagers
    {
        get { return _SceneManagers; }
        set { _SceneManagers = value; }
    }

    

    public List<SubSystemData> SubSystems
    {
        get { return _subSystems; }
        set { _subSystems = value; }
    }

    public string Version
    {
        get { return _version; }
        set { _version = value; }
    }

    public List<ViewComponentData> ViewComponents
    {
        get { return _viewComponents; }
        set { _viewComponents = value; }
    }

    public List<ElementData> ViewModels
    {
        get { return _viewModels; }
        set { _viewModels = value; }
    }

    public List<ViewData> Views
    {
        get { return _views; }
        set { _views = value; }
    }
    public Stack<IDiagramFilter> FilterStack
    {
        get
        {
            return _filterStack ?? (_filterStack = new Stack<IDiagramFilter>());
        }
        set { _filterStack = value; }
    }

    public void Initialize()
    {
        if (_persistedFilterStack.Count != (FilterStack.Count))
        {
            foreach (var filterName in _persistedFilterStack)
            {
                var filter = Filters.FirstOrDefault(p => p.Name == filterName);
                if (filter == null)
                {
                    _persistedFilterStack.Clear();
                    FilterStack.Clear();
                    break;
                }
                this.PushFilter(filter);
            }
        }
    }

    public void FilterPushed(IDiagramFilter filter)
    {
        if (!_persistedFilterStack.Contains(filter.Name))
            _persistedFilterStack.Add(filter.Name);
    }

    public void FilterPoped()
    {
        _persistedFilterStack.Remove(_persistedFilterStack.Last());
    }
}

public static class ElementDesignerDataExtensions
{
    public static void Prepare(this IElementDesignerData designerData)
    {
        designerData.RefactorCount = 0;
        foreach (var allDiagramItem in designerData.AllDiagramItems)
        {
            allDiagramItem.Data = designerData;
        }
        designerData.Initialize();
    }
    public static IEnumerable<IDiagramNode> FilterItems(this IElementDesignerData designerData, IEnumerable<IDiagramNode> allDiagramItems)
    {
        return designerData.FilterItems(designerData.CurrentFilter, allDiagramItems);
    }   
    public static void FilterLeave(this IElementDesignerData data)
    {
    }

    public static void ApplyFilter(this IElementDesignerData designerData)
    {
        designerData.UpdateLinks();
        //foreach (var item in DiagramItems)
        //{
        //    item.Filter = CurrentFilter;
        //}
        //UpdateLinks();
    }

    public static void CleanUpFilters(this IElementDesignerData designerData)
    {
        var diagramItems = designerData.AllDiagramItems.Select(p => p.Identifier);

        foreach (var diagramFilter in designerData.Filters)
        {
            var removeKeys = diagramFilter.Locations.Keys.Where(p => !diagramItems.Contains(p)).ToArray();
            foreach (var removeKey in removeKeys)
            {
                diagramFilter.Locations.Remove(removeKey);
            }
        }
        //UpdateLinks();
    }

    public static string GetUniqueName(this IElementDesignerData designerData, string name)
    {
        var tempName = name;
        var index = 1;
        while (designerData.AllDiagramItems.Any(p => p.Name.ToUpper() == tempName.ToUpper()))
        {
            tempName = name + index;
            index++;
        }
        return tempName;
    }

    public static IEnumerable<IDiagramNode> FilterItems(this IElementDesignerData designerData,IDiagramFilter filter, IEnumerable<IDiagramNode> allDiagramItems)
    {
        foreach (var item in allDiagramItems)
        {
            if (filter.IsAllowed(item, item.GetType()))
            {
                if (filter.ImportedOnly && filter != item)
                {
                    if (filter.Locations.Keys.Contains(item.Identifier))
                    {
                        yield return item;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }

    public static void PopFilter(this IElementDesignerData designerData,List<string> filterStack)
    {
        designerData.FilterLeave();
        //filterStack.Remove(designerData.FilterStack.Peek().Name);
        designerData.FilterStack.Pop();
        designerData.ApplyFilter();
    }

    public static void PopToFilter(this IElementDesignerData designerData, IDiagramFilter filter1)
    {
        while (designerData.CurrentFilter != filter1)
        {
            designerData.PopFilter(null);
        }
    }

    public static void PopToFilter(this IElementDesignerData designerData, string filterName)
    {
        while (designerData.CurrentFilter.Name != filterName)
        {
            designerData.PopFilter(null);
        }
    }

    public static void PushFilter(this IElementDesignerData designerData, IDiagramFilter filter)
    {
        designerData.FilterLeave();
        designerData.FilterStack.Push(filter);
        designerData.FilterPushed(filter);
        designerData.ApplyFilter();
    }



    public static void ReloadFilterStack(this IElementDesignerData designerData,List<string> filterStack )
    {
        if (filterStack.Count != (designerData.FilterStack.Count))
        {
            foreach (var filterName in filterStack)
            {
                var filter = designerData.Filters.FirstOrDefault(p => p.Name == filterName);
                if (filter == null)
                {
                    filterStack.Clear();
                    designerData.FilterStack.Clear();
                    break;
                }
                designerData.PushFilter(filter);
            }
        }
    }

    public static void UpdateLinks(this IElementDesignerData designerData)
    {
        designerData.CleanUpFilters();
        designerData.Links.Clear();

        var items = designerData.DiagramItems.SelectMany(p => p.Items).Where(p => designerData.CurrentFilter.IsItemAllowed(p, p.GetType())).ToArray();
        var diagramItems = designerData.DiagramItems.ToArray();
        foreach (var item in items)
        {
            designerData.Links.AddRange(item.GetLinks(diagramItems));
        }

        var diagramFilter = designerData.CurrentFilter as IDiagramNode;
        if (diagramFilter != null)
        {
            var diagramFilterItems = diagramFilter.Items.OfType<IDiagramNode>().ToArray();
            foreach (var diagramItem in diagramItems)
            {
                designerData.Links.AddRange(diagramItem.GetLinks(diagramFilterItems));
            }
            //foreach (var diagramFilterItem in diagramFilterItems)
            //{
            //    Links.AddRange(diagramFilterItem.GetLinks(diagramItems));
            //}
        }

        var models = designerData.DiagramItems.ToArray();

        foreach (var viewModelData in models)
        {
            //viewModelData.Filter = CurrentFilter;
            designerData.Links.AddRange(viewModelData.GetLinks(diagramItems));
        }
    }
    public static IEnumerable<ElementDataBase> GetAssociatedElementsInternal(this IElementDesignerData designerData, ElementDataBase data)
    {
        var derived = GetAllBaseItems(designerData, data);
        foreach (var viewModelItem in derived)
        {
            var element = GetElement(designerData,viewModelItem);
            if (element != null)
            {
                yield return element;
                var subItems = GetAssociatedElementsInternal(designerData, element);
                foreach (var elementDataBase in subItems)
                {
                    yield return elementDataBase;
                }
            }
        }
    }
    public static IEnumerable<IDiagramNode> FilterItems(this IElementDesignerData designerData, IDiagramFilter filter)
    {
        return FilterItems(designerData, filter, designerData.AllDiagramItems);
    }

    public static IEnumerable<IViewModelItem> GetAllBaseItems(this IElementDesignerData designerData, ElementDataBase data)
    {
        var current = data;
        while (current != null)
        {
            foreach (var item in current.Items)
            {
                if (item is IViewModelItem)
                {
                    yield return item as IViewModelItem;
                }
            }

            current = designerData.AllElements.FirstOrDefault(p => p.AssemblyQualifiedName == current.BaseTypeName);
        }
    }

    public static ElementDataBase[] GetAssociatedElements(this IElementDesignerData designerData, ElementDataBase data)
    {
        return GetAssociatedElementsInternal(designerData, data).Concat(new[] { data }).Distinct().ToArray();
    }

    public static ElementDataBase GetElement(this IElementDesignerData designerData, IViewModelItem item)
    {
        if (item.RelatedTypeName == null)
        {
            return null;
        }
        return designerData.AllElements.FirstOrDefault(p => p.Name == item.RelatedTypeName);
    }

    public static IEnumerable<IDiagramFilter> GetFilters(this IElementDesignerData designerData, IDiagramFilter filter)
    {
        //yield return DefaultFilter;
        //yield return SceneFlowFilter;

        foreach (var allDiagramItem in designerData.FilterItems(filter).OfType<IDiagramFilter>())
        {
            yield return allDiagramItem;
        }
    }

    //public static string GetUniqueName(this IElementDesignerData designerData, string name)
    //{
    //    var tempName = name;
    //    var index = 1;
    //    while (designerData.AllDiagramItems.Any(p => p.Name.ToUpper() == tempName.ToUpper()))
    //    {
    //        tempName = name + index;
    //        index++;
    //    }
    //    return tempName;
    //}

    public static ElementData GetViewModel(this IElementDesignerData designerData, string elementName)
    {
        return designerData.ViewModels.FirstOrDefault(p => p.Name == elementName);
    }

}