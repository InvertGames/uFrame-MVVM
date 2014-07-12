using Invert.uFrame.Editor;
using UnityEngine;

public interface IDiagramNodeItem : ISelectable, IJsonObject
{
    string Name { get; set; }
    string Highlighter { get; }
    string FullLabel { get; }
    string Identifier { get; }
    bool IsSelectable { get;}
    DiagramNode Node { get; set; }
    void Remove(IDiagramNode diagramNode);
    void Rename(IDiagramNode data, string name);
}