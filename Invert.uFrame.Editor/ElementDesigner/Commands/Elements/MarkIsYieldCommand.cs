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
                UnityEngine.Debug.Log("arg not found");
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
                UnityEngine.Debug.Log("arg not found");
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
}