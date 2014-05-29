using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewDrawer : DiagramItemDrawer<ViewData>
{
    private ElementDataBase _forElement;

    public override GUIStyle BackgroundStyle
    {
        get
        {

            return UFStyles.DiagramBox2;
        }
    }

    public ViewDrawer(ViewData data, ElementsDiagram diagram)
        : base(data, diagram)
    {
        Diagram = diagram;

    }

    public override bool AllowCollapsing
    {
        get { return true; }
    }


    private DiagramItemHeader _behavioursHeader;

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        yield break;
    }

    public override GUIStyle ItemStyle
    {
        get { return UFStyles.Item4; }
    }

    public override void DoubleClicked()
    {
        base.DoubleClicked();
        Diagram.Repository.NavigateToView(this.Data);
    }

    protected override void DrawSelectedItem(IDiagramSubItem item, ElementsDiagram diagram)
    {
        base.DrawSelectedItem(item, diagram);
    }
}