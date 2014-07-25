using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using UnityEngine;

public class ViewComponentDrawer : DiagramNodeDrawer<ViewComponentData>
{
    private NodeItemHeader _additiveScenesHeader;

    public override GUIStyle BackgroundStyle
    {
        get
        {

            return ElementDesignerStyles.DiagramBox6;
        }
    }

    public ViewComponentDrawer()
    {
    }

    public ViewComponentDrawer(ViewComponentData data, ElementsDiagram diagram)
        : base(data, diagram)
    {
        Diagram = diagram;
    }

    public override bool AllowCollapsing
    {
        get { return false; }
    }

    public NodeItemHeader AdditiveScenesHeader
    {
        get { return _additiveScenesHeader ?? (_additiveScenesHeader = new NodeItemHeader() { Label = "Additive Scenes", HeaderType = typeof(AdditiveSceneData) }); }
        set { _additiveScenesHeader = value; }
    }

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        if (!Data.Items.Any()) yield break;
        yield return new DiagramSubItemGroup()
        {
            Header = AdditiveScenesHeader,
            Items = Data.Items.ToArray()
        };
    }

    public override GUIStyle ItemStyle
    {
        get { return ElementDesignerStyles.Item4; }
    }

}