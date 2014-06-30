namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class DeleteItemCommand : EditorCommand<DiagramNodeItem>, IDiagramNodeItemCommand, IKeyBindable
    {
        public override string Name
        {
            get { return "Delete"; }
        }

        public override void Perform(DiagramNodeItem node)
        {
            if (node == null) return;
            node.Remove(node.Node);
        }

        public override string CanPerform(DiagramNodeItem node)
        {
            return null;
        }
    }
}