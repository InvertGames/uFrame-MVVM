using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ElementData : ElementDataBase, IDiagramFilter
{
    public override void Serialize(JSONClass cls)
    {
        base.Serialize(cls); 
        cls.Add("IsTemplate",new JSONData(IsTemplate));
        cls.Add("BaseType", new JSONData(_baseType));
        cls.Add("IsMultiInstance", new JSONData(_isMultiInstance));
    }

    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);
        _baseType = cls["BaseType"].Value;
        IsTemplate = cls["IsTemplate"].AsBool;
        _isMultiInstance = cls["IsMultiInstance"].AsBool;
    }

    [SerializeField]
    private string _baseType;

    [SerializeField]
    private FilterCollapsedDictionary _collapsedValues = new FilterCollapsedDictionary();

    [SerializeField]
    private List<ViewModelCollectionData> _collections = new List<ViewModelCollectionData>();

    [SerializeField]
    private List<ViewModelCommandData> _commands = new List<ViewModelCommandData>();

    [SerializeField]
    private FilterLocations _locations = new FilterLocations();

    [SerializeField]
    private List<ViewModelPropertyData> _properties = new List<ViewModelPropertyData>();

    [SerializeField]
    private bool _isTemplate;

    //public bool IsImportOnly
    //{
    //    get
    //    {
    //        if (CurrentViewModelType == null) return false;
    //        var attribute =
    //            CurrentViewModelType.GetCustomAttributes(typeof (DiagramInfoAttribute), false).FirstOrDefault() as
    //                DiagramInfoAttribute;
    //        if (attribute == null)
    //            return false;

    //        return attribute.DiagramName == Data.name;
    //    }
    //}

    public override string BaseTypeName
    {
        get
        {
            return _baseType ?? UFrameAssetManager.DesignerVMAssemblyName;
        }
        set
        {
            _baseType = value;
            Dirty = true;
        }
    }

    public override string SubTitle
    {
        get
        {
            if (string.IsNullOrEmpty(BaseTypeName))
            {
                return string.Empty;
            }
            if (BaseTypeShortName == "ViewModel")
            {
                return null;
            }
            return BaseTypeShortName;
            //return BaseTypeShortName ?? string.Empty;
        }
    }

    public FilterCollapsedDictionary CollapsedValues
    {
        get { return _collapsedValues; }
        set { _collapsedValues = value; }
    }

    public override ICollection<ViewModelCollectionData> Collections
    {
        get { return _collections; }
        set { _collections = value.ToList(); }
    }

    public override ICollection<ViewModelCommandData> Commands
    {
        get { return _commands; }
        set { _commands = value.ToList(); }
    }

    public bool ImportedOnly
    {
        get { return true; }
    }

    public IEnumerable<ViewComponentData> IncludedComponents
    {
        get
        {
            foreach (var viewComponentData in Data.ViewComponents)
            {
                if (AllBaseTypes.Any(p => p.Identifier == viewComponentData.ElementIdentifier))
                {
                    yield return viewComponentData;
                }
            }
        }
    }

    public IEnumerable<ViewData> IncludedViews
    {
        get
        {
            foreach (var v in Data.Views)
            {
                if (AllBaseTypes.Any(p => p.AssemblyQualifiedName == v.AssemblyQualifiedName))
                {
                    yield return v;
                }
            }
        }
    }

    public override string InfoLabel
    {
        get { return string.Format("Items: [{0}] {1}", Locations.Keys.Count - 1, base.InfoLabel ?? string.Empty); }
    }

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get
        {
            if (Data.CurrentFilter == this)
            {
                return IncludedViews.Cast<IDiagramNodeItem>().Concat(IncludedComponents.Cast<IDiagramNodeItem>());
            }
            return base.Items;
        }
    }

    public FilterLocations Locations
    {
        get { return _locations; }
        set { _locations = value; }
    }

    public override ICollection<ViewModelPropertyData> Properties
    {
        get { return _properties; }
        set { _properties = value.ToList(); }
    }

    public IEnumerable<ViewComponentData> ViewComponents
    {
        get
        {
            foreach (var viewComponentData in Data.ViewComponents)
            {
                if (viewComponentData.ElementIdentifier == this.Identifier ||
                    AllBaseTypes.Any(p => p.Identifier == viewComponentData.ElementIdentifier))
                {
                    yield return viewComponentData;
                }
            }
        }
    }

    public IEnumerable<ViewData> Views
    {
        get
        {
            foreach (var v in Data.Views)
            {
                if (v.ForAssemblyQualifiedName == this.AssemblyQualifiedName ||
                    AllBaseTypes.Any(p => p.AssemblyQualifiedName == v.AssemblyQualifiedName))
                {
                    yield return v;
                }
            }
        }
    }

    public override RenameRefactorer CreateRenameRefactorer()
    {
        return new RenameElementRefactorer(this);
    }

    public bool IsAllowed(object item, Type t)
    {
        
        if (item == this) return true;
        if (t == typeof(SubSystemData)) return false;
        //if (t == typeof(ExternalSubsystem)) return false;
        if (t == typeof(SceneManagerData)) return false;
        if (t == typeof(ViewComponentData)) return true;
        if (t == typeof(ViewData)) return true;
        if (t == typeof(ElementData)) return false;
        if (t == typeof(EnumData)) return true;
        if (t == typeof (IDiagramNodeItem)) return false;
        return true;
    }

    public bool IsItemAllowed(object item, Type t)
    {
        //if (typeof(IViewModelItem).IsAssignableFrom(t)) return true;

        return true;
    }

    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);

        foreach (var vm in Data.Elements)
        {
            if (vm.BaseTypeShortName == Name)
            {
                vm.RemoveLink(vm);
            }
        }

        foreach (var elementData in Data.Elements)
        {
            foreach (var diagramSubItem in elementData.ViewModelItems)
            {
                if (diagramSubItem.RelatedTypeName == this.Name)
                {
                    diagramSubItem.RemoveLink(this);
                }
            }
        }
        foreach (var viewData in Data.Views)
        {
            if (viewData.ForAssemblyQualifiedName == this.AssemblyQualifiedName)
            {
                viewData.ForAssemblyQualifiedName = null;
            }
        }
        foreach (var viewComponentData in Data.ViewComponents)
        {
            if (viewComponentData.ElementIdentifier == this.Identifier)
            {
                viewComponentData.ElementIdentifier = null;
            }
        }
    }

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get
        {
            foreach (var property in Properties)
            {
                yield return property;
            }
            foreach (var command in Commands)
            {
                yield return command;
            }
            foreach (var collection in Collections)
            {
                yield return collection;
            }
        }
        set
        {
            var stuff = value.ToArray();
            Properties = stuff.OfType<ViewModelPropertyData>().ToList();
            Commands = stuff.OfType<ViewModelCommandData>().ToList();
            Collections = stuff.OfType<ViewModelCollectionData>().ToList();
        }
    }

    public bool IsTemplate
    {
        get { return _isTemplate; }
        set { _isTemplate = value; }
    }

    public override void RemoveLink(IDiagramNode target)
    {
        var elementData = target as ElementData;
        if (elementData != null)
            elementData.BaseTypeName = null;

        var viewData = target as ViewData;
        if (viewData != null)
        {
            viewData.ForAssemblyQualifiedName = null;
        }
        var viewComponent = target as ViewComponentData;
        if (viewComponent != null)
        {
            viewComponent.ElementIdentifier = null;
        }
    }
}