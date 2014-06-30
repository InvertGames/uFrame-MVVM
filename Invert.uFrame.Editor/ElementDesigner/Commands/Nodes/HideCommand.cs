namespace Invert.uFrame.Editor.ElementDesigner.Commands
{

    public class HideCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "File"; }
        }
        public override void Perform(IDiagramNode node)
        {
            node.Data.CurrentFilter.Locations.Remove(node.Identifier);
        }

        public override string CanPerform(IDiagramNode node)
        {
            if (node == null) return "Diagram Item must not be null.";
            return null;
        }
    }
}