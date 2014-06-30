namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class RenameCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "File"; }
        }

        public override decimal Order
        {
            get { return -1; }
        }

        public override void Perform(IDiagramNode node)
        {
            node.BeginEditing();
        }

        public override string CanPerform(IDiagramNode node)
        {
            if (node == null) return "Invalid argument";
            return null;
        }
    }
}