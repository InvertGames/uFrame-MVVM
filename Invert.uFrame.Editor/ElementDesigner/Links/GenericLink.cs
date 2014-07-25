using Invert.Common;
using UnityEngine;

public class GenericLink : BeizureLink
{
    public IDiagramNode Element { get; set; }

    public IDiagramNode Node { get; set; }

    public override ISelectable Source { get { return Node; } }

    public override ISelectable Target { get { return Element; } }

    public override NodeCurvePointStyle EndStyle
    {
        get { return base.StartStyle; }
    }

    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.Settings.AssociationLinkColor;
    }
}