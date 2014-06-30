using System;
using System.Collections.Generic;
using System.Linq;
using Invert.uFrame.Editor;
using UnityEngine;

public interface ISubSystemData : IDiagramNode
{
    List<string> Imports { get; set; }
    FilterLocations Locations { get; set; }
}

public static class SubsystemExtensions
{
    public static IEnumerable<ISubSystemData> GetAllImportedSubSystems(this ISubSystemData subsystem,IElementDesignerData data)
    {
        var subSystem = data.AllDiagramItems.OfType<SubSystemData>()
            .Where(p => subsystem.Imports.Contains(p.Identifier));

        foreach (var subSystemData in subSystem)
        {
            yield return subSystemData;
            foreach (var systemData in subSystemData.GetAllImportedSubSystems(subSystemData.OwnerData))
            {
                yield return systemData;
            }
        }
    }
    public static IEnumerable<string> GetAllImports(this ISubSystemData subsystem)
    {
        return subsystem.GetAllImportedSubSystems(subsystem.OwnerData).SelectMany(p => p.Imports).Concat(subsystem.Imports);
    }
    public static IEnumerable<IDiagramNodeItem> GetIncludedItems(this ISubSystemData subsystem)
    {
        foreach (var allDiagramItem in subsystem.OwnerData.AllDiagramItems.OfType<ISubSystemData>())
        {
            if (subsystem.Imports.Contains(allDiagramItem.Identifier))
            {
                foreach (var item in allDiagramItem.GetSubItems())
                {
                    yield return item;
                }
            }
        }
    }
    public static IEnumerable<IDiagramNodeItem> GetSubItems(this ISubSystemData subsystem)
    {
        var items =
            subsystem.OwnerData.AllDiagramItems.OfType<ISubSystemType>()
                .Where(p => subsystem.Locations.Keys.Contains(p.Identifier))
                .Cast<IDiagramNodeItem>();

        foreach (var item in items)
            yield return item;
    }
    public static IEnumerable<ViewModelCommandData> GetIncludedCommands(this ISubSystemData subsystem)
    {
        return subsystem.GetIncludedElements().Where(p => !p.IsMultiInstance).SelectMany(p => p.Commands);
    }
    public static IEnumerable<ElementData> GetIncludedElements(this ISubSystemData subsystem)
    {
        var list = new List<ElementData>();
        foreach (var diagramSubItem in subsystem.GetSubItems().OfType<ElementData>())
        {
            list.Add(diagramSubItem);
        }
        foreach (var allDiagramItem in subsystem.Data.AllDiagramItems.OfType<SubSystemData>())
        {
            if (subsystem.Imports.Contains(allDiagramItem.Identifier))
            {
                foreach (var nested in allDiagramItem.GetIncludedElements())
                {
                    list.Add(nested);
                }
            }
        }
        return list.Distinct();
    }
}
[Serializable]
public class SubSystemData : DiagramNode, IDiagramFilter, ISubSystemData
{
    public override void Serialize(JSONClass cls)
    {
        base.Serialize(cls);
        cls.AddPrimitiveArray("Imports", _imports, i => new JSONData(i));
    }

    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);

        _imports = cls["Imports"].AsArray.DeserializePrimitiveArray(n => n.Value).ToList();
    }

    [SerializeField]
    private FilterCollapsedDictionary _collapsedValues = new FilterCollapsedDictionary();

    [SerializeField]
    private List<string> _imports = new List<string>();

    [SerializeField]
    private FilterLocations _locations = new FilterLocations();

    public FilterCollapsedDictionary CollapsedValues
    {
        get { return _collapsedValues; }
        set { _collapsedValues = value; }
    }

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get { yield break; }
        set { }
    }

    public bool ImportedOnly
    {
        get { return true; }
    }

    public List<string> Imports
    {
        get { return _imports; }
        set { _imports = value; }
    }


    public override string InfoLabel
    {
        get { return string.Format("Items: [{0}]", Locations.Keys.Count - 1); }
    }

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get
        {
            if (this == Data.CurrentFilter)
            {
                foreach (var diagramSubItem in this.GetIncludedItems()) yield return diagramSubItem;
                //foreach (var diagramSubItem1 in GetSubItems(data)) yield return diagramSubItem1;
            }
            else
            {
                yield break;
                //var items = SubItems.OfType<ElementDataBase>().Where(p => !p.IsMultiInstance).SelectMany(p => p.Commands);
                //foreach (var item in items)
                //{
                //    yield return item;
                //}
                //foreach (var diagramSubItem in GetIncludedItems(data)) yield return diagramSubItem;
                //foreach (var diagramSubItem1 in GetSubItems(data)) yield return diagramSubItem1;
            }
        }
    }

    public override string Label
    {
        get { return Name; }
    }

    public FilterLocations Locations
    {
        get { return _locations; }
        set { _locations = value; }
    }

    public override bool CanCreateLink(IDrawable target)
    {
        var subsystem = target as SubSystemData;
        if (subsystem != null)
        {
            return !this.GetAllImports().Contains(subsystem.Identifier);
            //return subsystem != this && !subsystem.Imports.Contains(Identifier);
        }
        return target is SceneManagerData;
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        var subSystem = target as SubSystemData;
        if (subSystem != null)
        {
            subSystem.Imports.Add(Identifier);
            return;
        }
        var sceneManagerData = target as SceneManagerData;
        if (sceneManagerData != null)
        {
            sceneManagerData.SubSystemIdentifier = this.Identifier;
        }
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        foreach (var import in Imports)
        {
            var item = nodes.OfType<SubSystemData>().FirstOrDefault(p => p.Identifier == import);
            if (item != null)
            {
                yield return new SubSystemLink()
                {
                    Start = item,
                    Finish = this
                };
            }
        }
        var scenes = nodes.OfType<SceneManagerData>().Where(p => p.SubSystemIdentifier == Identifier);
        foreach (var sceneManagerData in scenes)
        {
            yield return new SceneManagerSystemLink()
            {
                SceneManager = sceneManagerData,
                SubSystem = this
            };
        }
    }

    public bool IsAllowed(object item, Type t)
    {
        if (item == this) return true;

        if (t == typeof(SubSystemData)) return false;
        if (t == typeof(SceneManagerData)) return false;
        if (t == typeof(ViewComponentData)) return false;
        if (t == typeof(ViewData)) return false;

        return true;
    }

    public bool IsItemAllowed(object item, Type t)
    {
        if (t == typeof(ViewComponentData)) return false;
        if (t == typeof(ViewData)) return false;
        return true;
    }

    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);
    }

    public override void RemoveLink(IDiagramNode target)
    {
        var subSystem = target as SubSystemData;
        if (subSystem != null)
        {
            subSystem.Imports.Remove(Identifier);
        }
        var sceneManager = target as SceneManagerData;
        if (sceneManager != null)
        {
            sceneManager.SubSystemIdentifier = null;
        }
    }
}