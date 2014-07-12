using UnityEngine;

public class ViewLink : BeizureLink
{
    public ViewData Data { get; set; }

    public IDiagramNode Element { get; set; }

    public override ISelectable Source { get { return Element; } }

    public override ISelectable Target { get { return Data; } }

    public override Rect StartRect
    {
        get
        {
            var source = Source as IDiagramNode;
            if (source != null)
            {
                return source.HeaderPosition;
            }
            return base.StartRect;
        }
    }

    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.Settings.ViewLinkColor;
    }

    public override NodeCurvePointStyle EndStyle
    {
        get { return base.StartStyle; }
    }

}