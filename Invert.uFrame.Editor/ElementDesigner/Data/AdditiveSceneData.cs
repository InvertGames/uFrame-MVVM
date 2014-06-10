using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AdditiveSceneData : IDiagramNodeItem
{
    [SerializeField]
    private string _name;
    public Rect Position { get; set; }
    public string Label { get; private set; }

    public void CreateLink(IDiagramNode container, IDrawable target)
    {
        
    }

    public bool CanCreateLink(IDrawable target)
    {
        return false;
    }

    public IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        yield break;
    }

    public bool IsSelected { get; set; }
    public void RemoveLink(IDiagramNode target)
    {
        
    }

    public Vector2[] ConnectionPoints { get; set; }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Highlighter { get; private set; }
    public string FullLabel { get; private set; }

    [SerializeField]
    private string _identifier;
    public string Identifier { get { return string.IsNullOrEmpty(_identifier) ? (_identifier = Guid.NewGuid().ToString()) : _identifier; } }
    public bool IsSelectable { get { return true; } }


    public void Remove(IDiagramNode diagramNode)
    {
        
    }

    public void Rename( IDiagramNode data, string name)
    {
        Name = name;
    }
}