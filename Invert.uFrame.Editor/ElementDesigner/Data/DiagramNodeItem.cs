using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class DiagramNodeItem : IDiagramNodeItem
{
    public virtual void Serialize(JSONClass cls)
    {
        cls.Add("Name", new JSONData(_name));
        cls.Add("Identifier", new JSONData(_identifier));
    }

    public virtual void Deserialize(JSONClass cls)
    {
        _name = cls["Name"].Value;
        _identifier = cls["Identifier"].Value;
    }

    [SerializeField]
    private string _identifier;

    [NonSerialized]
    private bool _isSelected;

    [SerializeField]
    private string _name = string.Empty;

    [NonSerialized]
    private List<Refactorer> _refactorings;
    [NonSerialized]
    private RenameRefactorer _renameRefactorer;
    [NonSerialized]
    private string _oldName;
    [NonSerialized]
    private Rect _position;

    public abstract string FullLabel { get; }

    public virtual string Highlighter { get { return null; } }

    public string Identifier { get { return string.IsNullOrEmpty(_identifier) ? (_identifier = Guid.NewGuid().ToString()) : _identifier; } }

    public virtual bool IsSelectable { get { return true; } }
    public DiagramNode Node  { get; set; }

    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            if (value == false && _isSelected)
            {
                EndEditing();
            }
            else if (value == true && !_isSelected)
            {
                BeginEditing();
            }
            _isSelected = value;
        }
    }

    public abstract string Label { get; }

    public virtual string Name
    {
        get { return _name; }
        set { _name = Regex.Replace(value, "[^a-zA-Z0-9_.]+", ""); }
    }

    public string OldName
    {
        get { return _oldName; }
        set { _oldName = value; }
    }

    public Rect Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public List<Refactorer> Refactorings
    {
        get { return _refactorings ?? (_refactorings = new List<Refactorer>()); }
        set { _refactorings = value; }
    }

    public RenameRefactorer RenameRefactorer
    {
        get { return _renameRefactorer; }
        set { _renameRefactorer = value; }
    }

    public virtual void BeginEditing()
    {
        if (RenameRefactorer == null)
        {
            RenameRefactorer = CreateRenameRefactorer();
        }
        OldName = Name;
    }

    public abstract bool CanCreateLink(IDrawable target);

    public abstract void CreateLink(IDiagramNode container, IDrawable target);

    public virtual RenameRefactorer CreateRenameRefactorer()
    {
        return null;
    }

    public virtual void EndEditing()
    {
        if (OldName != Name)
        {
            if (RenameRefactorer == null)
            {
                return;
            }

            RenameRefactorer.Set(this);
            if (!Refactorings.Contains(RenameRefactorer))
            {
                Refactorings.Add(RenameRefactorer);
            }
        }
    }

    public abstract IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] diagramNode);

    public void RefactorApplied()
    {
        Refactorings.Clear();
    }

    public abstract void Remove(IDiagramNode diagramNode);

    public abstract void RemoveLink(IDiagramNode target);

    public virtual void Rename(IDiagramNode data, string name)
    {
        Name = name;
    }
    
   
}