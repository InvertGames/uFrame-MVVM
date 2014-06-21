using System.Collections;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class DiagramNode : IDiagramNode, IRefactorable
{
    public void Serialize(JSONClass cls)
    {
        cls.Add("Name", new JSONData(_name));
        cls.Add("IsCollapsed", new JSONData(_isCollapsed));
        cls.Add("Identifier", new JSONData(_identifier));

        var itemsArray = new JSONArray();
        foreach (var diagramNodeItem in ContainedItems)
        {
            var nodeItemClass = new JSONClass { { "Type", diagramNodeItem.GetType().Name } };
            diagramNodeItem.Serialize(nodeItemClass);

            itemsArray.Add(nodeItemClass);
        }
        cls.Add("Items", itemsArray);

    }

    /// <summary>
    /// The items that should be persisted with this diagram node.
    /// </summary>
    public abstract IEnumerable<IDiagramNodeItem> ContainedItems { get; }

    [NonSerialized]
    private IElementDesignerData _data;

    [SerializeField]
    private string _identifier;

    [SerializeField]
    private bool _isCollapsed;

    private Vector2 _location;

    [SerializeField]
    private string _name;

    private Rect _position;

    private List<Refactorer> _refactorings;

    public IEnumerable<Refactorer> AllRefactorers
    {
        get { return Refactorings.Concat(Items.OfType<IRefactorable>().SelectMany(p => p.Refactorings)); }
    }

    public virtual string AssemblyQualifiedName
    {
        get
        {
            return UFrameAssetManager.DesignerVMAssemblyName.Replace("ViewModel", Name);
        }
    }

    public Vector2[] ConnectionPoints { get; set; }

    public IElementDesignerData Data
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;
            if (value != null)
            {
                //if (!value.Locations.Keys.Contains(Identifier))
                //{
                //    value.Locations[this] = Location;
                //}
                _location = value.CurrentFilter.Locations[this];
                _isCollapsed = value.CurrentFilter.CollapsedValues[this];
                Dirty = true;
            }
        }
    }

    public bool Dirty { get; set; }

    public IDiagramFilter Filter
    {
        get { return Data.CurrentFilter; }
    }

    public string FullLabel { get { return Name; } }

    public Rect HeaderPosition { get; set; }

    public string Highlighter { get { return null; } }

    public string Identifier { get { return string.IsNullOrEmpty(_identifier) ? (_identifier = Guid.NewGuid().ToString()) : _identifier; } }

    public virtual string InfoLabel
    {
        get
        {
            return null;
            //var count = AllRefactorers.Count();
            //if (count == 0) return null;
            //return string.Format("Refactors: {0}", count);
        }
    }

    public virtual bool IsCollapsed
    {
        get { return _isCollapsed; }
        set
        {
            _isCollapsed = value;
            Filter.CollapsedValues[this] = value;
            Dirty = true;
        }
    }

    public bool IsEditing { get; set; }

    public bool IsSelectable { get { return true; } }

    public bool IsSelected { get; set; }

    public abstract IEnumerable<IDiagramNodeItem> Items { get; }

    public abstract string Label { get; }

    public Vector2 Location
    {
        get
        {
            //if (Filter == this)
            //{
            //    return new Vector2((Screen.width / 2f) - (Position.width / 2f), (Screen.height / 2f) - (Position.height / 2f));
            //}
            return _location;
        }
        set
        {
            _location = value;
            Filter.Locations[this] = value;
            if (_location.x < 0)
                _location.x = 0;
            if (_location.y < 0)
                _location.y = 0;
            Dirty = true;
        }
    }

    public virtual string Name
    {
        get { return _name; }
        set
        {
            _name = Regex.Replace(value, "[^a-zA-Z0-9_.]+", "");
            Dirty = true;
        }
    }

    public virtual string OldName
    {
        get;
        set;
    }

    public Rect Position
    {
        get
        {
            return _position;
        }
        set { _position = value; }
    }

    public List<Refactorer> Refactorings
    {
        get { return _refactorings ?? (_refactorings = new List<Refactorer>()); }
        set { _refactorings = value; }
    }

    public RenameRefactorer RenameRefactorer { get; set; }

    public virtual bool ShouldRenameRefactor { get { return true; } }

    protected DiagramNode()
    {
    }

    public virtual void BeginEditing()
    {
        if (RenameRefactorer == null)
        {
            RenameRefactorer = CreateRenameRefactorer();
        }
        OldName = Name;
        IsEditing = true;
    }

    [DiagramContextMenu("Rename", 0)]
    public void BeginRename()
    {
        BeginEditing();
    }

    public abstract bool CanCreateLink(IDrawable target);

    public abstract void CreateLink(IDiagramNode container, IDrawable target);

    public virtual RenameRefactorer CreateRenameRefactorer()
    {
        return null;
    }

    public virtual bool EndEditing()
    {
        IsEditing = false;
        if (Data.GetDiagramItems().Count(p => p.Name == Name) > 1)
        {
            Name = OldName;
            return false;
        }

        if (OldName != Name)
        {
            if (RenameRefactorer == null)
            {
                return false;
            }

            RenameRefactorer.Set(this);
            if (!Refactorings.Contains(RenameRefactorer))
            {
                Refactorings.Add(RenameRefactorer);
                Data.RefactorCount++;
            }
        }
        return true;
    }

    public abstract IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes);

    public virtual void RefactorApplied()
    {
        Refactorings.Clear();
        RenameRefactorer = null;
    }

    public void Remove(IDiagramNode diagramNode)
    {
        Filter.Locations.Remove(this.Identifier);
    }

    //[DiagramContextMenu("Delete From All")]
    public virtual void RemoveFromDiagram()
    {
        Data.RefactorCount -= Refactorings.Count;
    }

    [DiagramContextMenu("Hide", 1)]
    public void RemoveFromFilter(IElementDesignerData data)
    {
        Filter.Locations.Remove(this.Identifier);
    }

    public abstract void RemoveLink(IDiagramNode target);

    public void Rename(IDiagramNode data, string name)
    {
        Rename(name);
    }

    public virtual void Rename(string newName)
    {
        Name = newName;
    }


}