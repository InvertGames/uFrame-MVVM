//using System.Collections.Generic;
//using Invert.uFrame.Editor;
//using Invert.uFrame.Editor.ElementDesigner;
//using UnityEngine;

//public class ExternalNodeDrawer : DiagramNodeDrawer<ExternalSubsystem>
//{
//    public override bool AllowCollapsing
//    {
//        get { return false; }
//    }

//    public override GUIStyle BackgroundStyle
//    {
//        get { return ElementDesignerStyles.DiagramBox5; }
//    }

//    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
//    {
//        yield break;
//    }

//    public IEditorCommand DoubleClickedCommand
//    {
//        get { return Container.Resolve<IEditorCommand>("ExternalSubsystem_DoubleClick"); }
//    }

//    public override void DoubleClicked()
//    {
//        base.DoubleClicked();
//        if (DoubleClickedCommand != null)
//        {
//            Diagram.ExecuteCommand(DoubleClickedCommand);
//        }
//    }
//}