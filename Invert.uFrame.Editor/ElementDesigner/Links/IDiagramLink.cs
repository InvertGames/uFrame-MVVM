public interface IDiagramLink
{
    ISelectable Source { get; }

    ISelectable Target { get; }

    void Draw(ElementsDiagram diagram);

    void DrawPoints(ElementsDiagram diagram);
}