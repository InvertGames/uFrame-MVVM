namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class MarkIsTemplateCommand : EditorCommand<ElementData>, IDiagramNodeCommand
    {
        public override string Name
        {
            get { return "Is Template"; }
        }

        public override string CanPerform(ElementData arg)
        {
            if (arg == null)
            {
                return "Must be an element to perform this operation.";
            }

            return null;
        }

        public override bool IsChecked(ElementData arg)
        {
            return arg.IsTemplate;
        }

        public override void Perform(ElementData arg)
        {
            arg.IsTemplate = !arg.IsTemplate;
        }
    }
    public class MarkIsYieldCommand : EditorCommand<ViewModelCommandData>, IDiagramNodeItemCommand
    {
        public override string Name
        {
            get { return "Is Yield Command"; }
        }

        public override string CanPerform(ViewModelCommandData arg)
        {
            if (arg == null)
            {
                return "Must be a command to perform this operation.";
            }
                
            return null;
        }

        public override bool IsChecked(ViewModelCommandData arg)
        {
            return arg.IsYield;
        }

        public override void Perform(ViewModelCommandData arg)
        {
            arg.IsYield = !arg.IsYield;
        }
    }
    public class RemoveNodeItemCommand : EditorCommand<ElementsDiagram>, IDiagramNodeItemCommand
    {
        public override string Name
        {
            get { return "Remove Item"; }
        }


        public override bool IsChecked(ElementsDiagram arg)
        {
            return false;
        }

        public override void Perform(ElementsDiagram arg)
        {
 
            var diagramNodeItem = arg.SelectedItem as IDiagramNodeItem;
            if (diagramNodeItem != null)
                diagramNodeItem.Remove(arg.SelectedData);
        }

        public override string CanPerform(ElementsDiagram node)
        {
            return null;
        }
    }
}