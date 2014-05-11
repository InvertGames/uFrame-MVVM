using UnityEngine;

public class AssociationLink : BeizureLink
{
    public IDiagramItem Element { get; set; }

    public IViewModelItem Item { get; set; }

    public override ISelectable Source { get { return Item; } }

    public override ISelectable Target { get { return Element; } }
    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.AssociationLinkColor;
    }
}