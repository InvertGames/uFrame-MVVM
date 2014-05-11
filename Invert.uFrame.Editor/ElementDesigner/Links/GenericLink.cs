using UnityEngine;

public class GenericLink : BeizureLink
{
    public IDiagramItem Element { get; set; }

    public IDiagramItem Item { get; set; }

    public override ISelectable Source { get { return Item; } }

    public override ISelectable Target { get { return Element; } }

    public override NodeCurvePointStyle EndStyle
    {
        get { return base.StartStyle; }
    }

    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.AssociationLinkColor;
    }
}