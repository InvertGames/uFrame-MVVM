using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

public class ViewDrawer : DiagramNodeDrawer<ViewData>
{
    private ElementDataBase _forElement;

    public ViewDrawer()
    {
    }

    public override GUIStyle BackgroundStyle
    {
        get
        {

            return ElementDesignerStyles.DiagramBox2;
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


    private NodeItemHeader _behavioursHeader;
    private NodeItemHeader _propertiesHeader;

    public NodeItemHeader PropertiesHeader
    {
        get
        {
            if (_propertiesHeader == null)
            {
                _propertiesHeader = Container.Resolve<NodeItemHeader>();

                _propertiesHeader.Label = "Properties";

                _propertiesHeader.HeaderType = typeof(ViewModelPropertyData);
                _propertiesHeader.AddCommand = Container.Resolve<AddViewPropertyCommand>();
            }
            return _propertiesHeader;
        }
        set { _propertiesHeader = value; }
    }

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        yield return new DiagramSubItemGroup()
        {
            Header = PropertiesHeader,
            Items = Data.ContainedItems.ToArray()
        };
    }

    public override GUIStyle ItemStyle
    {
        get { return ElementDesignerStyles.Item4; }
    }

    [Inject("ViewDoubleClick")]
    public IEditorCommand DoubleClickCommand { get; set; }

    public override void DoubleClicked()
    {
        base.DoubleClicked();
        if (DoubleClickCommand != null)
        {
            Diagram.ExecuteCommand(DoubleClickCommand);
        }
    }

    public override bool ShowSubtitle
    {
        get { return true; }
    }
    
  
}