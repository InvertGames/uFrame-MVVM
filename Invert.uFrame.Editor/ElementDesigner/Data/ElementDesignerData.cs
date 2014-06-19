using System.Security.Cryptography;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[Serializable]
public class FilterState
{
    [NonSerialized]
    private Stack<IDiagramFilter> _filterStack;

 
    //// Filters
    //public IDiagramFilter CurrentFilter
    //{
    //    get
    //    {
            
    //        return FilterStack.Peek();
    //    }
    //}


    [SerializeField, HideInInspector]
    private DefaultFilter _defaultFilter = new DefaultFilter();

    public DefaultFilter DefaultFilter
    {
        get { return _defaultFilter; }
        set { _defaultFilter = value; }
    }
    public Stack<IDiagramFilter> FilterStack
    {
        get
        {
            return _filterStack ?? (_filterStack = new Stack<IDiagramFilter>());
        }
        set { _filterStack = value; }
    }

    public List<string> _persistedFilterStack;

    public void FilterPushed(IDiagramFilter filter)
    {
        if (!_persistedFilterStack.Contains(filter.Name))
            _persistedFilterStack.Add(filter.Name);
    }

    public void FilterPoped(IDiagramFilter pop)
    {
        _persistedFilterStack.Remove(pop.Name);
    }


    public JSONNode Serialize()
    {
        var cls = new JSONClass();
        return cls;
    }

    public void Deserialize(JSONNode node)
    {
        
    }

    public void Reload(IElementDesignerData elementDesignerData)
    {
        if (_persistedFilterStack.Count != (FilterStack.Count))
        {
            foreach (var filterName in _persistedFilterStack)
            {
                var filter = elementDesignerData.GetFilters().FirstOrDefault(p => p.Name == filterName);
                if (filter == null)
                {
                    _persistedFilterStack.Clear();
                    FilterStack.Clear();
                    break;
                }
                elementDesignerData.PushFilter(filter);
            }
        }
    }
}
public interface IElementDesignerData
{
    // Settings
    ElementDiagramSettings Settings { get; }
    // Basic Information
    string Name { get; }
    string Version { get; set; }

    // Not Persisted
    int RefactorCount { get; set; }

    FilterState FilterState { get; set; }
    // Filters
    IDiagramFilter CurrentFilter { get; }
   
   

    // Queries
    IEnumerable<IDiagramNode> AllDiagramItems { get; }
    //IEnumerable<IDiagramNode> GetImportableItems();

    // Filter Stuff
    List<Refactorer> Refactorings { get; }
    SceneFlowFilter SceneFlowFilter { get;  }

    // Node Data
    IEnumerable<SceneManagerData> SceneManagers { get; }
    IEnumerable<SubSystemData> SubSystems { get; }
    IEnumerable<ViewComponentData> ViewComponents { get; }
    IEnumerable<ElementData> Elements { get; }
    IEnumerable<ViewData> Views { get; }
    //IEnumerable<ImportedElementData> ImportedElements { get; }
    IEnumerable<EnumData> Enums { get; }

    // Dynamically loaded
    List<IDiagramLink> Links { get; }

    /// <summary>
    /// Should be called when first loaded.
    /// </summary>
    void Initialize();



    void AddNode(IDiagramNode data);
    void RemoveNode(IDiagramNode enumData);
}

[Serializable]
public class ElementDiagramSettings
{
    [SerializeField]
    private Color _associationLinkColor = Color.white;
    [SerializeField,HideInInspector]
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

    public JSONNode Serialize()
    {
        return new JSONClass();
    }
}

[SerializeField]
public class ElementDesignerData : ScriptableObject,  IElementDesignerData
{
    public IDiagramFilter CurrentFilter
    {
        get
        {
            if (FilterState.FilterStack.Count < 1)
            {
                return SceneFlowFilter;
            }
            return FilterState.FilterStack.Peek();
        }
    }
    public ElementDiagramSettings Settings
    {
        get { return _settings ?? (_settings = new ElementDiagramSettings()); }
        set { _settings = value; }
    }


    [SerializeField, HideInInspector]
    private SceneFlowFilter _sceneFlowFilter = new SceneFlowFilter();
    public SceneFlowFilter SceneFlowFilter
    {
        get
        {
            return _sceneFlowFilter;
        }
        set { _sceneFlowFilter = value; }
    }


    [SerializeField, HideInInspector]
    private List<EnumData> _enums = new List<EnumData>();

    //[NonSerialized]
    //private Stack<IDiagramFilter> _filterStack;

    //[SerializeField, HideInInspector]
    //private List<ImportedElementData> _importedElements = new List<ImportedElementData>();

   
    [NonSerialized]
    private List<IDiagramLink> _links = new List<IDiagramLink>();

    //[SerializeField, HideInInspector]
    //private List<string> _persistedFilterStack = new List<string>();

    //[SerializeField, HideInInspector]
    //private List<PluginData> _pluginItems = new List<PluginData>();



    

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
    [SerializeField]
    private FilterState _filterState = new FilterState();


    public IEnumerable<IDiagramNode> AllDiagramItems
    {
        get
        {
            return
                ViewModels.Cast<IDiagramNode>()
                    //.Concat(ImportedElements.Cast<IDiagramNode>())
                    .Concat(Enums.Cast<IDiagramNode>())
                    .Concat(Views.Cast<IDiagramNode>())
                    .Concat(SceneManagers.Cast<IDiagramNode>())
                    .Concat(SubSystems.Cast<IDiagramNode>())
                    .Concat(ViewComponents.Cast<IDiagramNode>()
                );
        }
    }
    IEnumerable<EnumData> IElementDesignerData.Enums
    {
        get { return Enums; }
    }
    public List<EnumData> Enums
    {
        get { return _enums; }
        set { _enums = value; }
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


    IEnumerable<SceneManagerData> IElementDesignerData.SceneManagers
    {
        get { return SceneManagers; }
    }
    public List<SceneManagerData> SceneManagers
    {
        get { return _SceneManagers; }
        set { _SceneManagers = value; }
    }

    IEnumerable<SubSystemData> IElementDesignerData.SubSystems
    {
        get { return SubSystems; }
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

    IEnumerable<ViewComponentData> IElementDesignerData.ViewComponents
    {
        get { return ViewComponents; }
    }
    public List<ViewComponentData> ViewComponents
    {
        get { return _viewComponents; }
        set { _viewComponents = value; }
    }

    IEnumerable<ElementData> IElementDesignerData.Elements
    {
        get { return ViewModels; }
    }
    public List<ElementData> ViewModels
    {
        get { return _viewModels; }
        set { _viewModels = value; }
    }

    IEnumerable<ViewData> IElementDesignerData.Views
    {
        get { return Views; }
    }
    public List<ViewData> Views
    {
        get { return _views; }
        set { _views = value; }
    }

    public FilterState FilterState
    {
        get { return _filterState; }
        set { _filterState = value; }
    }

    public void Initialize()
    {
        //if (FilterState.FilterStack.Count < 1)
        //{
        //    FilterState.FilterStack.Push(SceneFlowFilter);
        //}
        FilterState.Reload(this);
    }

    public void AddNode(IDiagramNode data)
    {
        TryNode<ElementData>(data,n=>ViewModels.Add(n));
        TryNode<EnumData>(data,n=>Enums.Add(n));
        TryNode<SubSystemData>(data,n=>SubSystems.Add(n));
        TryNode<ViewData>(data,n=>Views.Add(n));
        TryNode<ViewComponentData>(data,n=>ViewComponents.Add(n));
        TryNode<SceneManagerData>(data,n=>SceneManagers.Add(n));
    }

    public void TryNode<TNodeData>(IDiagramNode node, Action<TNodeData> action) where TNodeData : class, IDiagramNode
    {
        var element = node as TNodeData;
        if (element != null)
        {
            action(element);
        }
        
    }
    public void RemoveNode(IDiagramNode data)
    {
        TryNode<ElementData>(data, n => ViewModels.Remove(n));
        TryNode<EnumData>(data, n => Enums.Remove(n));
        TryNode<SubSystemData>(data, n => SubSystems.Remove(n));
        TryNode<ViewData>(data, n => Views.Remove(n));
        TryNode<ViewComponentData>(data, n => ViewComponents.Remove(n));
        TryNode<SceneManagerData>(data, n => SceneManagers.Remove(n));
    }
}

public static class ElementDesignerDataExtensions
{
    public static IEnumerable<IDiagramNode> GetImportableItems(this IElementDesignerData t)
    {
        //return AllowedDiagramItems;
        return
            t.GetAllowedDiagramItems()
                .Where(p => !t.CurrentFilter.Locations.Keys.Contains(p.Identifier))
                .ToArray();
        //return items.Where(p => !Filters.Any(x => x.Locations.Keys.Contains(p.Identifier)));
    }
    public static IEnumerable<ElementDataBase> GetAllElements(this IElementDesignerData t)
    {
        return t.AllDiagramItems.OfType<ElementDataBase>();
    }

    public static IEnumerable<IDiagramNode> GetAllowedDiagramItems(this IElementDesignerData t)
    {
        return t.AllDiagramItems.Where(p => t.CurrentFilter.IsAllowed(p, p.GetType()));
    }
    public static IEnumerable<IDiagramNode> GetDiagramItems(this IElementDesignerData t)
    {
        return t.FilterItems(t.AllDiagramItems);
    }

    public static IEnumerable<ElementDataBase> GetElements(this IElementDesignerData t)
    {
        return t.GetDiagramItems().OfType<ElementDataBase>();
    }

    public static IEnumerable<IDiagramFilter> GetFilterPath(this IElementDesignerData t)
    {
        return t.FilterState.FilterStack.Reverse();
    }

    public static IEnumerable<IDiagramFilter> GetFilters(this IElementDesignerData t)
    {
        return t.AllDiagramItems.OfType<IDiagramFilter>();
    }
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

        foreach (var diagramFilter in designerData.GetFilters())
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

        designerData.FilterState.FilterPoped(designerData.FilterState.FilterStack.Pop());
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
        designerData.FilterState.FilterStack.Push(filter);
        designerData.FilterState.FilterPushed(filter);
        designerData.ApplyFilter();
    }



    public static void ReloadFilterStack(this IElementDesignerData designerData,List<string> filterStack )
    {
        if (filterStack.Count != (designerData.FilterState.FilterStack.Count))
        {
            foreach (var filterName in filterStack)
            {
                var filter = designerData.GetFilters().FirstOrDefault(p => p.Name == filterName);
                if (filter == null)
                {
                    filterStack.Clear();
                    designerData.FilterState.FilterStack.Clear();
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

        var items = designerData.GetDiagramItems().SelectMany(p => p.Items).Where(p => designerData.CurrentFilter.IsItemAllowed(p, p.GetType())).ToArray();
        var diagramItems = designerData.GetDiagramItems().ToArray();
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

        var models = designerData.GetDiagramItems().ToArray();

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

            current = designerData.GetAllElements().FirstOrDefault(p => p.AssemblyQualifiedName == current.BaseTypeName);
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
        return designerData.GetAllElements().FirstOrDefault(p => p.Name == item.RelatedTypeName);
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
        return designerData.Elements.FirstOrDefault(p => p.Name == elementName);
    }

}


public class JsonElementDesignerData : IElementDesignerData
{
    private List<IDiagramNode> _nodes = new List<IDiagramNode>();
    private SceneFlowFilter _sceneFlowFilter = new SceneFlowFilter();
    private List<IDiagramLink> _links = new List<IDiagramLink>();
    public IDiagramFilter CurrentFilter
    {
        get
        {
            if (FilterState.FilterStack.Count < 1)
            {
                return SceneFlowFilter;
            }
            return FilterState.FilterStack.Peek();
        }
    }
    public List<IDiagramNode> Nodes
    {
        get { return _nodes; }
        set { _nodes = value; }
    }

    public JSONNode Serialize()
    {
        return Serialize(this);
    }

    public static JSONNode Serialize(IElementDesignerData data)
    {
        // The root class for the diagram data
        var root = new JSONClass();

        // Serialize every node
        var nodeArray = new JSONArray();
        foreach (var diagramNode in data.AllDiagramItems)
        {
            // The diagram node class
            var nodeClass = new JSONClass();
            // Store the type information for reloading
            nodeClass.Add("Type", diagramNode.GetType().AssemblyQualifiedName);
            // Call the serialize method on the node
            diagramNode.Serialize(nodeClass);
            // Add it to the array
            nodeArray.Add(nodeClass);
        }
        // Name of the diagram
        root.Add("Name", new JSONData(data.Name));
        // Version of the diagram
        root.Add("Version", new JSONData(data.Version));
        // Add The nodes array
        root.Add("Nodes",nodeArray);
        // Add the filter state
        root.Add("FilterState",data.FilterState.Serialize());
        // Add the settings
        root.Add("Settings",data.Settings.Serialize());

        return root;
    }

    public ElementDiagramSettings Settings { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public int RefactorCount { get; set; }
    public FilterState FilterState { get; set; }

    public IEnumerable<IDiagramNode> AllDiagramItems
    {
        get { return this.Nodes; }
    }

    private List<Refactorer> _refactorings = new List<Refactorer>();
    
    public List<Refactorer> Refactorings
    {
        get { return _refactorings; }
    }

    public SceneFlowFilter SceneFlowFilter
    {
        get { return _sceneFlowFilter; }
        set { _sceneFlowFilter = value; }
    }
    public IEnumerable<SceneManagerData> SceneManagers
    {
        get { return Nodes.OfType<SceneManagerData>(); }
    }
    public IEnumerable<SubSystemData> SubSystems { get { return Nodes.OfType<SubSystemData>(); } }
    public IEnumerable<ViewComponentData> ViewComponents { get { return Nodes.OfType<ViewComponentData>(); } }
    public IEnumerable<ElementData> Elements { get { return Nodes.OfType<ElementData>(); } }
    public IEnumerable<ViewData> Views { get { return Nodes.OfType<ViewData>(); } }
    public IEnumerable<EnumData> Enums { get { return Nodes.OfType<EnumData>(); } }

    public List<IDiagramLink> Links
    {
        get { return _links; }
        set { _links = value; }
    }

    public void Initialize()
    {
        //if (FilterState.FilterStack.Count < 1)
        //{
        //    FilterState.FilterStack.Push(SceneFlowFilter);
        //}
    }

    public void AddNode(IDiagramNode data)
    {
        Nodes.Add(data);
    }

    public void RemoveNode(IDiagramNode enumData)
    {
        Nodes.Remove(enumData);
    }
}

public class ConvertToJSON : EditorCommand<IElementDesignerData>, IToolbarCommand
{
    public override void Perform(IElementDesignerData node)
    {
        UnityEngine.Debug.Log(JsonElementDesignerData.Serialize(node));
    }

    public override string CanPerform(IElementDesignerData node)
    {
        if (node == null) return "Problem with node";
        return null;
    }

    public override decimal Order
    {
        get { return -1; }
    }

    public ToolbarPosition Position
    {
        get
        {
            return ToolbarPosition.Left;
        }
    }
}
public class JsonRepository : IElementsDataRepository
{
    public IElementDesignerData LoadDiagram(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new NullReferenceException("Path can't be null.");
        var asset = AssetDatabase.LoadAssetAtPath(path, typeof (TextAsset)) as TextAsset;
        if (asset == null)
        {
            throw new Exception("File could not be loaded.  It must be a TextAsset.");
        }
        var json = asset.text;
        
        var data = new JsonElementDesignerData();

        return null;
    }

    public void SaveDiagram(IElementDesignerData data)
    {
        throw new NotImplementedException();
    }

    public void RecordUndo(IElementDesignerData data, string title)
    {
        throw new NotImplementedException();
    }

    public void MarkDirty(IElementDesignerData data)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetProjectDiagrams()
    {
        throw new NotImplementedException();
    }

    public void CreateNewDiagram()
    {
        throw new NotImplementedException();
    }
}