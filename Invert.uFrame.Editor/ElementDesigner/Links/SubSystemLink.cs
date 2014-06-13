using UnityEngine;

public class SubSystemLink : BeizureLink
{
    public SubSystemData Finish { get; set; }

    public override ISelectable Source { get { return Start; } }

    public  SubSystemData Start { get; set; }

    public override ISelectable Target { get { return Finish; } }
    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.Settings.SubSystemLinkColor;
    }
}