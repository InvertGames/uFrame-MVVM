using System;
using System.Collections.Generic;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.Refactoring;
using UnityEngine;

public interface IDiagramNode : ISelectable,IDiagramNodeItem
{
    string SubTitle { get; }
    /// <summary>
    /// The label that sits above the node providing additional insight.
    /// </summary>
    string InfoLabel { get; }
    /// <summary>
    /// Is this node collapsed or expanded
    /// </summary>
    bool IsCollapsed { get; set; }

    /// <summary>
    /// Is this node currently in edit mode/ rename mode.
    /// </summary>
    bool IsEditing { get; set; }

    /// <summary>
    /// Any child list items of the node
    /// </summary>
    IEnumerable<IDiagramNodeItem> Items { get; }
    
    /// <summary>
    /// The current position of the node on the diagram
    /// </summary>
    Vector2 Location { get; set; }
    /// <summary>
    /// An assembly name representing this node if applicable
    /// </summary>
    string AssemblyQualifiedName { get; }

    /// <summary>
    /// Is this node dirty/modified and should its bounds be recalculated.
    /// </summary>
    bool Dirty { get; set; }

    /// <summary>
    /// Begins renaming the node.
    /// </summary>
    /// <param name="newName"></param>
    void Rename(string newName);

    /// <summary>
    /// Remove the node from the diagram.  Usually justs calls RemoveNode on the OwnerData
    /// </summary>
    void RemoveFromDiagram();

    /// <summary>
    /// The position of the header or title of the node.  Calculated by the drawer.
    /// </summary>
    Rect HeaderPosition { get; set; } 
    /// <summary>
    /// The current element data displaying this node
    /// </summary>
    IElementDesignerData Data { get; set; }
    /// <summary>
    /// The element data that owns this node
    /// </summary>
    IElementDesignerData OwnerData { get;}
    /// <summary>
    /// The current filter
    /// </summary>
    IDiagramFilter Filter { get; }
    /// <summary>
    /// The name that was used when the last save occured.
    /// </summary>
    string OldName { get; set; }

    /// <summary>
    /// The items that should be persisted with this diagram node.
    /// </summary>
    IEnumerable<IDiagramNodeItem> ContainedItems { get; }

    bool IsNewNode { get; set; }


    /// <summary>
    /// Begin the rename process
    /// </summary>
    void BeginEditing();
    /// <summary>
    /// Apply changes to the renaming of the node.
    /// </summary>
    /// <returns>Could it successfully rename the node.</returns>
    bool EndEditing();

    
}

public interface IRefactorable
{
    IEnumerable<Refactorer> Refactorings { get;  }
    void RefactorApplied();
}