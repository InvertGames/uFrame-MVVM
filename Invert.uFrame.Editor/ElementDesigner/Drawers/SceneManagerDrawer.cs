using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using UnityEngine;

public class SceneManagerDrawer : DiagramNodeDrawer<SceneManagerData>
{
    private NodeItemHeader _transitionsHeader;

    //public override float Padding
    //{
    //    get { return 15; }
    //}
    public SceneManagerDrawer()
    {
    }

    public override GUIStyle BackgroundStyle
    {
        get
        {

            return ElementDesignerStyles.DiagramBox7;
        }
    }

    public SceneManagerDrawer(SceneManagerData data, ElementsDiagram diagram)
        : base(data, diagram)
    {
        Diagram = diagram;

    }

    public override bool AllowCollapsing
    {
        get { return true; }
    }

    public NodeItemHeader TransitionsHeader
    {
        get { return _transitionsHeader ?? (_transitionsHeader = new NodeItemHeader() { Label = "Transitions", HeaderType = typeof(SceneManagerData) }); }
        set { _transitionsHeader = value; }
    }

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        if (!Data.Items.Any()) yield break;
        yield return new DiagramSubItemGroup()
        {
            Header = TransitionsHeader,
            Items = Data.Items.ToArray()
        };
    }

    public override GUIStyle ItemStyle
    {
        get { return ElementDesignerStyles.Item4; }
    }

}