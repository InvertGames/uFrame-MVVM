using UnityEngine;

public class SubSystemLink : BeizureLink
{
    public ISubSystemData Finish { get; set; }

    public override ISelectable Source { get { return Start; } }

    public  ISubSystemData Start { get; set; }

    public override ISelectable Target { get { return Finish; } }
    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.Settings.SubSystemLinkColor;
    }
}