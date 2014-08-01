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
    private NodeItemHeader _bindingsHeader;

    public NodeItemHeader BindingsHeader
    {
        get
        {
            if (_bindingsHeader == null)
            {
                _bindingsHeader = Container.Resolve<NodeItemHeader>();
                _bindingsHeader.Label = "Bindings";
                _bindingsHeader.HeaderType = typeof(string);
                _bindingsHeader.AddCommand = Container.Resolve<AddBindingCommand>();
            }
            return _bindingsHeader;
        }
        set { _propertiesHeader = value; }
    }

    public NodeItemHeader PropertiesHeader
    {
        get
        {
            if (_propertiesHeader == null)
            {
                _propertiesHeader = Container.Resolve<NodeItemHeader>();

                _propertiesHeader.Label = "2-Way Properties";

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
        var vForElement = Data.ViewForElement;
        if (vForElement != null)
        {
            var existing =
                Data.BindingMethods.Select(p => (IDiagramNodeItem) (new BindingDiagramItem(p.Name)));
            var adding =
                Data.NewBindings.Select(p => (IDiagramNodeItem) (new BindingDiagramItem("[Added] " + p.MethodName)));
            yield return new DiagramSubItemGroup()
            {
                Header = BindingsHeader,
                Items = existing.Concat(adding).ToArray()
            };
        }
       
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

public class BindingDiagramItem : DiagramNodeItem
{
    public BindingDiagramItem(string methodName)
    {
        MethodName = methodName;
    }

    public string MethodName { get; set; }
    public override string FullLabel
    {
        get { return MethodName; }
    }

    public override string Label
    {
        get { return MethodName; }
    }
    
    public override bool CanCreateLink(IDrawable target)
    {
        return false;
    }

    public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] diagramNode)
    {
        yield break;
    }

    public override void Remove(IDiagramNode diagramNode)
    {
        
    }

    public override void RemoveLink(IDiagramNode target)
    {
        
    }

    public override void CreateLink(IDiagramNode container, IDrawable target)
    {
        
    }
}