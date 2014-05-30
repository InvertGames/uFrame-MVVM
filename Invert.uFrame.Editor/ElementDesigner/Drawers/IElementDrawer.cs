using System.Collections.Generic;

public interface IElementDrawer
{
    string ShouldFocus { get; }
    void Draw(ElementsDiagram elementsDiagram);
    void CalculateBounds();
    bool IsSelected { get; }
    IDiagramItem Model { get; set; }
    IEnumerable<IDiagramSubItem> Items { get; }
    ElementsDiagram Diagram { get; set; }
    void DoubleClicked();
}