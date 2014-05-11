using UnityEngine;

public class InheritanceLink : BeizureLink
{
    public ElementDataBase Base { get; set; }

    public ElementDataBase Derived { get; set; }

    public override ISelectable Source { get { return Derived; } }

    public override ISelectable Target { get { return Base; } }

    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.InheritanceLinkColor;
    }
}