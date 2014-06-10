using UnityEngine;

public interface IDiagramNodeItem : ISelectable
{
    string Name { get; set; }
    string Highlighter { get; }
    string FullLabel { get; }
    string Identifier { get; }
    bool IsSelectable { get;}
    void Remove(IDiagramNode diagramNode);
    void Rename(IDiagramNode data, string name);


}