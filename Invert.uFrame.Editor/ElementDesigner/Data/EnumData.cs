using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnumData : DiagramNode
{
    [SerializeField]
    private List<EnumItem> _enumItems = new List<EnumItem>();

    public List<EnumItem> EnumItems
    {
        get { return _enumItems; }
        set { _enumItems = value; }
    }

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get { return EnumItems.Cast<IDiagramNodeItem>(); }
    }

    public override void EndEditing()
    {
        base.EndEditing();
        foreach (var item in Data.Elements.SelectMany(p => p.Properties).Where(p => p.RelatedTypeName == OldName))
        {
            item.RelatedType = AssemblyQualifiedName;
        }
        foreach (var item in Data.Elements.SelectMany(p => p.Commands).Where(p => p.RelatedTypeName == OldName))
        {
            item.RelatedType = AssemblyQualifiedName;
        }
        foreach (var item in Data.Elements.SelectMany(p => p.Collections).Where(p => p.RelatedTypeName == OldName))
        {
            item.RelatedType = AssemblyQualifiedName;
        }
    }

  
    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);
    }


    public override string InfoLabel
    {
        get { return null; }
    }

    public override string Label { get { return Name; }}

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get { return EnumItems.Cast<IDiagramNodeItem>(); }
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        
    }

    public override bool CanCreateLink(IDrawable target)
    {
        return false;
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        yield break;
    }


    public override void RemoveLink(IDiagramNode target)
    {
        
    }
}