using System.Collections.Generic;
using UnityEngine;

public interface IDrawable
{
    Rect Position { get; set; }
    string Label { get; }


    void CreateLink(IDiagramNode container, IDrawable target);
    bool CanCreateLink(IDrawable target);
    IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes);
}