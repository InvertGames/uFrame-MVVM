using UnityEngine;

public class ViewLink : BeizureLink
{
    public ViewData Data { get; set; }

    public IDiagramNode Element { get; set; }

    public override ISelectable Source { get { return Element; } }

    public override ISelectable Target { get { return Data; } }
    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.ViewLinkColor;
    }

    public override NodeCurvePointStyle EndStyle
    {
        get { return base.StartStyle; }
    }

}