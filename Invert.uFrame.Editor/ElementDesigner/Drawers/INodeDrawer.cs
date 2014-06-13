using System;
using System.Collections.Generic;
using Invert.uFrame.Editor;

public interface INodeDrawer 
{
    string ShouldFocus { get; }
    void Draw(ElementsDiagram elementsDiagram);
    void CalculateBounds();
    bool IsSelected { get; }
    IDiagramNode Model { get; set; }
    IEnumerable<IDiagramNodeItem> Items { get; }
    ElementsDiagram Diagram { get; set; }
    Type CommandsType { get; }
    void DoubleClicked();
}