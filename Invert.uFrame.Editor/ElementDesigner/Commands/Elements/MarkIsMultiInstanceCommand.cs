namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class MarkIsMultiInstanceCommand : EditorCommand<ElementData>, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "Flags"; }
        }
        public override string Name
        {
            get { return "Has Multiple Instances?"; }
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
            return arg.IsMultiInstance;
        }

        public override void Perform(ElementData arg)
        {
            arg.IsMultiInstance = !arg.IsMultiInstance;
        }
    }
}