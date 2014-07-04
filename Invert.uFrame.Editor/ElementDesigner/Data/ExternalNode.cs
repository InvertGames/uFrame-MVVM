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
    
    public T ExternalDiagramNode
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
        return ExternalDiagramNode.CanCreateLink(target);
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        return ExternalDiagramNode.GetLinks(nodes);
    }

    public override string Name
    {
        get { return ExternalDiagramNode.Name; }
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

public class ExternalSubsystem2 : ExternalNode<SubSystemData>, ISubSystemData
{

    public override IEnumerable<IDiagramNodeItem> ContainedItems { get; set; }
    public override bool CanCreateLink(IDrawable target)
    {
        return ExternalDiagramNode.CanCreateLink(target);
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        var subSystem = target as ISubSystemData;
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

    public override void RemoveLink(IDiagramNode target)
    {
        ExternalDiagramNode.RemoveLink(target);
    }

    public List<string> Imports
    {
        get { return ExternalDiagramNode.Imports; }
        set { ExternalDiagramNode.Imports = value; }
        //get { return _imports; }
        //set { _imports = value; }
    }

    public FilterLocations Locations
    {
        get { return ExternalDiagramNode.Locations; }
        set { ExternalDiagramNode.Locations = value; }
    }
}

public class ReferenceNode : DiagramNode
{
    private IDiagramNode _node;
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
        set { }
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

    public IDiagramNode ExternalDiagramNode
    {
        get
        {

            //UnityEngine.Debug.Log(ExternalDiagramIdentifier);
            if (ExternalDiagram == null)
            {
                throw new Exception("External diagram could not be found.");
            }
            return _node ?? (_node = ExternalDiagram.AllDiagramItems.FirstOrDefault(p => p.Identifier == ExternalNodeIdentifier));
        }
    }

    public override IEnumerable<IDiagramNodeItem> Items
    {
        get { throw new NotImplementedException(); }
    }

    public override string Label
    {
        get { throw new NotImplementedException(); }
    }

    public override bool CanCreateLink(IDrawable target)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        throw new NotImplementedException();
    }

    public override void RemoveLink(IDiagramNode target)
    {
        throw new NotImplementedException();
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        throw new NotImplementedException();
    }

    //public override IEnumerable<IDiagramNodeItem> ContainedItems { get; set; }
}