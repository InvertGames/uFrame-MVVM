using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

public class DiagramEnumDrawer : DiagramNodeDrawer<EnumData>
{
    private NodeItemHeader _itemsHeader;

    public override GUIStyle BackgroundStyle
    {
        get
        {

            return ElementDesignerStyles.DiagramBox9;
        }
    }

    //public override GUIStyle ItemStyle
    //{
    //    get { return ElementDesignerStyles.Item6; }
    //}
    public DiagramEnumDrawer()
    {
    }

    public DiagramEnumDrawer(EnumData data, ElementsDiagram diagram)
        : base(data, diagram)
    {

    }

    public NodeItemHeader ItemsHeader
    {
        get
        {

            if (_itemsHeader == null)
            {
                _itemsHeader = Container.Resolve<NodeItemHeader>();
                _itemsHeader.Label = "Items";
                _itemsHeader.HeaderType = typeof(EnumData);
                _itemsHeader.AddCommand = Container.Resolve<AddEnumItemCommand>();
            }

            return _itemsHeader;
        }
        set { _itemsHeader = value; }
    }

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        yield return new DiagramSubItemGroup()
        {
            Header = ItemsHeader,
            Items = Data.EnumItems.Cast<IDiagramNodeItem>().ToArray()
        };
    }

    protected override void DrawSelectedItem(IDiagramNodeItem nodeItem, ElementsDiagram diagram)
    {
        base.DrawSelectedItem(nodeItem, diagram);

    }
}