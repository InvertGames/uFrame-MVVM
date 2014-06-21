using System.Collections.Generic;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using UnityEngine;

public interface IDiagramNode : ISelectable,IDiagramNodeItem
{
    string InfoLabel { get; }
    bool IsCollapsed { get; set; }
    bool IsEditing { get; set; }
    IEnumerable<IDiagramNodeItem> Items { get; }
    //string Name { get; set; }
    Vector2 Location { get; set; }
    string AssemblyQualifiedName { get; }
    bool Dirty { get; set; }
    void Rename(string newName);
    void RemoveFromDiagram();
    Rect HeaderPosition { get; set; } 
    IElementDesignerData Data { get; set; }
    IDiagramFilter Filter { get; }
    string OldName { get; set; }

    /// <summary>
    /// The items that should be persisted with this diagram node.
    /// </summary>
    IEnumerable<IDiagramNodeItem> ContainedItems { get; }

    void BeginEditing();

    bool EndEditing();
    //void Serialize(JSONClass nodeClass);
}

public interface IRefactorable
{
    List<Refactorer> Refactorings { get;  }
    void RefactorApplied();
}