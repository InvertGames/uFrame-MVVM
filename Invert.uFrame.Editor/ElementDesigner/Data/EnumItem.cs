using System;
using System.Collections.Generic;
using Invert.uFrame.Editor;
using UnityEngine;

[Serializable]
public class EnumItem : IDiagramNodeItem
{
    public void Serialize(JSONClass cls)
    {
        cls.Add("Name", new JSONData(_name));
        cls.Add("Identifier", new JSONData(_identifier));
    }

    public void Deserialize(JSONClass cls)
    {
        _name = cls["Name"].Value;
        _identifier = cls["Identifier"].Value;
    }

  
    [SerializeField]
    private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Highlighter
    {
        get { return null; }
    }

    public string FullLabel { get { return  Name; } }
    public string Identifier{ get { return string.IsNullOrEmpty(_identifier) ? (_identifier = Guid.NewGuid().ToString()) : _identifier;}}
    public bool IsSelectable { get { return true; } }
    public DiagramNode Node { get; set; }

    [SerializeField]
    private string _identifier;
    public void Remove(IDiagramNode diagramNode)
    {
        var data = diagramNode as EnumData;
        data.EnumItems.Remove(this);
        data.Dirty = true;
    }

    public void Rename(IDiagramNode data, string name)
    {
        Name = name;
    }

   

    public Vector2[] ConnectionPoints { get; set; }

    public Rect Position { get; set; }

    public string Label
    {
        get { return Name; }
    }

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
        throw new NotImplementedException();
    }
}