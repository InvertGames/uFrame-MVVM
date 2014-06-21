using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SubSystemData : DiagramNode, IDiagramFilter
{
 
    [SerializeField]
    private List<string> _imports = new List<string>();

    public override string Label
    {
        get { return Name; }
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        var subSystem = target as SubSystemData;
        if (subSystem != null)
        {

            subSystem.Imports.Add(this.Identifier);
            return;
        }
        var SceneManager = target as SceneManagerData;
        if (SceneManager != null)
        {
            SceneManager.SubSystemIdentifier = this.Identifier;
        }
    }

    public override bool CanCreateLink(IDrawable target)
    {
        var subsystem = target as SubSystemData;
        if (subsystem != null)
        {
            return !AllImports.Contains(subsystem.Identifier);
            //return subsystem != this && !subsystem.Imports.Contains(Identifier);
        }
        return target is SceneManagerData;
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
        foreach (var SceneManagerData in scenes)
        {
            yield return new SceneManagerSystemLink()
            {
                SceneManager = SceneManagerData,
                SubSystem = this
            };
        }
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

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get
        {


            if (this == Data.CurrentFilter)
            {
                foreach (var diagramSubItem in IncludedItems) yield return diagramSubItem;
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

    public IEnumerable<IDiagramNodeItem> SubItems
    {
        get
        {
            var items =
                Data.AllDiagramItems.OfType<ISubSystemType>()
                    .Where(p => Locations.Keys.Contains(p.Identifier))
                    .Cast<IDiagramNodeItem>();

            foreach (var item in items)
                yield return item;
        }
    }

    public IEnumerable<ViewModelCommandData> IncludedCommands
    {
        get { return IncludedElements.Where(p=>!p.IsMultiInstance).SelectMany(p => p.Commands); }
    } 
    public IEnumerable<ElementDataBase> IncludedElements
    {
        get
        {
            var list = new List<ElementDataBase>();
            foreach (var diagramSubItem in SubItems.OfType<ElementDataBase>())
            {
                list.Add(diagramSubItem);
            }
            foreach (var allDiagramItem in Data.AllDiagramItems.OfType<SubSystemData>())
            {
                if (Imports.Contains(allDiagramItem.Identifier))
                {
                    foreach (var nested in allDiagramItem.IncludedElements)
                    {
                        list.Add(nested);
                    }
                }
            }
            return list.Distinct();
        }
    }
    [DiagramContextMenu("Print Includes")]
    public void PrintIncludedElements()
    {
        Debug.Log(string.Join(Environment.NewLine,IncludedElements.Select(p=>p.Name).ToArray()));
    }

    //public override bool IsCollapsed
    //{
    //    get { return true; }
    //    set
    //    {
            
    //    }
    //}

    public IEnumerable<string> AllImports
    {
        get
        {
            return AllImportedSubSystems.SelectMany(p => p.Imports).Concat(Imports);
        }
    } 
    public IEnumerable<SubSystemData> AllImportedSubSystems
    {
        get
        {
            var subSystem = Data.AllDiagramItems.OfType<SubSystemData>().Where(p => Imports.Contains(p.Identifier));
            foreach (var subSystemData in subSystem)
            {
                yield return subSystemData;
                foreach (var systemData in subSystemData.AllImportedSubSystems)
                {
                    yield return systemData;
                }
            }
        }
    } 
    public IEnumerable<IDiagramNodeItem> IncludedItems
    {
        get
        {
            foreach (var allDiagramItem in Data.AllDiagramItems.OfType<SubSystemData>())
            {
                if (Imports.Contains(allDiagramItem.Identifier))
                {
                    foreach (var item in allDiagramItem.SubItems)
                    {
                        yield return item;
                    }
                }
            }
        }
    }


    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);
    }

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get { yield break; }
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

    public FilterLocations Locations
    {
        get { return _locations; }
        set { _locations = value; }
    }

    [SerializeField]
    private FilterLocations _locations = new FilterLocations();

    [SerializeField]
    private FilterCollapsedDictionary _collapsedValues = new FilterCollapsedDictionary();

    public FilterCollapsedDictionary CollapsedValues
    {
        get { return _collapsedValues; }
        set { _collapsedValues = value; }
    }

    public string InfoLabel
    {
        get { return string.Format("Items: [{0}]", Locations.Keys.Count - 1); }
    }

    public bool IsAllowed(object item, Type t)
    {
        if (item == this) return true;
        
        if (t == typeof (SubSystemData)) return false;
        if (t == typeof (SceneManagerData)) return false;
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
}