using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DiagramEnumDrawer : DiagramItemDrawer<EnumData>
{
    public override GUIStyle BackgroundStyle
    {
        get
        {

            return UFStyles.DiagramBox9;
        }
    }

    //public override GUIStyle ItemStyle
    //{
    //    get { return UFStyles.Item6; }
    //}

    public DiagramEnumDrawer(EnumData data, ElementsDiagram diagram)
        : base(data, diagram)
    {
        ItemsHeader = new DiagramItemHeader()
        {
            Label = "EnumItems",
            HeaderType = typeof(EnumData)
        };
        ItemsHeader.OnAddItem += ItemsHeaderOnOnAddItem;
    }

    private void ItemsHeaderOnOnAddItem()
    {
        Undo.RecordObject(Diagram.Data, "Add New Enum");
        Data.EnumItems.Add(new EnumItem()
        {
            Name = "Start" + (Data.EnumItems.Count + 1)
        });
        CalculateBounds();
        Diagram.Refresh(true);
        EditorUtility.SetDirty(Diagram.Data);
    }

    public DiagramItemHeader ItemsHeader { get; set; }

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        yield return new DiagramSubItemGroup()
        {
            Header = ItemsHeader,
            Items = Data.EnumItems.Cast<IDiagramSubItem>().ToArray()
        };
    }

    protected override void DrawSelectedItem(IDiagramSubItem item, ElementsDiagram diagram)
    {
        base.DrawSelectedItem(item, diagram);

    }
}