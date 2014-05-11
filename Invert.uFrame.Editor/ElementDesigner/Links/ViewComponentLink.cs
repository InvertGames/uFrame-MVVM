using UnityEngine;

public class ViewComponentLink : BeizureLink
{
    public ViewComponentData Derived { get; set; }

    public override ISelectable Source { get { return Base; } }

    public override ISelectable Target { get { return Derived; } }

    public ViewComponentData Base { get; set; }

    public override Rect EndRect
    {
        get { return Derived.Position; }
    }
    public override Rect StartRect
    {
        get { return Base.Position; }
    }
    //public override NodeCurvePointStyle EndStyle
    //{
    //    get { return base.StartStyle; }
    //}

    public override Color GetColor(ElementsDiagram diagram)
    {
        return diagram.Data.SceneManagerLinkColor;
    }
}