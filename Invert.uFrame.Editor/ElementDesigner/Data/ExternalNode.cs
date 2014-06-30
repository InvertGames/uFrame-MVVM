using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Invert.uFrame.Editor;

public abstract class ExternalNode<T> : DiagramNode where T : DiagramNode,ISubSystemData
{
    private T _node;
    private string _externalNodeIdentifier;
    private string _externalDiagramIdentifier;

    public override void Serialize(Invert.uFrame.Editor.JSONClass cls)
    {
        base.Serialize(cls);
        cls.Add("ExternalNodeIdentifier", _externalNodeIdentifier);
        cls.Add("ExternalDiagramIdentifier", _externalDiagramIdentifier);
    }

    public override IEnumerable<IDiagramNodeItem> ContainedItems
    {
        get { yield break; }
    }

    public override void Deserialize(JSONClass cls)
    {
        base.Deserialize(cls);
        _externalNodeIdentifier = cls["ExternalNodeIdentifier"].Value;
        _externalDiagramIdentifier = cls["ExternalDiagramIdentifier"].Value;
    }

    public string ExternalNodeIdentifier
    {
        get { return _externalNodeIdentifier; }
        set { _externalNodeIdentifier = value; }
    }

    public string ExternalDiagramIdentifier
    {
        get { return _externalDiagramIdentifier; }
        set { _externalDiagramIdentifier = value; }
    }

    private IElementDesignerData _externalDesignerData;
    public IElementDesignerData ExternalDiagram
    {
        get
        {
            if (_externalDesignerData == null)
            {
                _externalDesignerData = UFrameAssetManager.Diagrams.FirstOrDefault(p => p.Identifier == ExternalDiagramIdentifier);
            }
            return _externalDesignerData;
        }
    }

    public T Node
    {
        get
        {

            //UnityEngine.Debug.Log(ExternalDiagramIdentifier);
            if (ExternalDiagram == null)
            {
                throw new Exception("External diagram could not be found.");
            }
            return _node ?? (_node = ExternalDiagram.AllDiagramItems.OfType<T>().FirstOrDefault(p => p.Identifier == ExternalNodeIdentifier));
        }
    }

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get { yield break; }
    }

    public override string InfoLabel
    {
        get { return "External: " + ExternalDiagram.Name; }
    }

    public override string Label
    {
        get { return  Name; }
    }
    
    public override bool CanCreateLink(IDrawable target)
    {
        return Node.CanCreateLink(target);
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        return Node.GetLinks(nodes);
    }

    public override string Name
    {
        get { return Node.Name; }
    }
    public override IElementDesignerData OwnerData
    {
        get { return ExternalDiagram; }
    }

    public override void RemoveFromDiagram()
    {
        base.RemoveFromDiagram();
        Data.RemoveNode(this);
    }
}

public class ExternalSubsystem : ExternalNode<SubSystemData>, ISubSystemData
{

    public override IEnumerable<IDiagramNodeItem> ContainedItems { get; set; }
    public override bool CanCreateLink(IDrawable target)
    {
        return Node.CanCreateLink(target);
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        Node.CreateLink(container,target);
    }

    public override void RemoveLink(IDiagramNode target)
    {
        Node.RemoveLink(target);
    }

    public List<string> Imports
    {
        get { return Node.Imports; }
        set { Node.Imports = value; }
        //get { return _imports; }
        //set { _imports = value; }
    }

    public FilterLocations Locations
    {
        get { return Node.Locations; }
        set { Node.Locations = value; }
    }
}
