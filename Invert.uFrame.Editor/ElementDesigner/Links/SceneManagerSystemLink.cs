using UnityEngine;

public class SceneManagerSystemLink : BeizureLink
{
    public SceneManagerData SceneManager { get; set; }

    public override ISelectable Source { get { return SubSystem; } }

    public override ISelectable Target { get { return SceneManager; } }

    public ISubSystemData SubSystem { get; set; }

    public override Rect EndRect
    {
        get { return SceneManager.HeaderPosition; }
    }
    public override Rect StartRect
    {
        get { return SubSystem.HeaderPosition; }
    }
    public override NodeCurvePointStyle EndStyle
    {
        get { return base.StartStyle; }
    }

    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.Settings.SceneManagerLinkColor;
    }
}