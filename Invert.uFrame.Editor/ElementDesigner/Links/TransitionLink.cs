using UnityEngine;

public class TransitionLink : BeizureLink
{
    public SceneManagerTransition From { get; set; }

    public override ISelectable Source { get { return From; } }

    public override ISelectable Target { get { return To; } }
    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.Settings.TransitionLinkColor;
    }

    public SceneManagerData To { get; set; }

    public override Rect EndRect
    {
        get { return To.HeaderPosition; }
    }

    public override Rect StartRect
    {
        get { return From.Position; }
    }
    
}