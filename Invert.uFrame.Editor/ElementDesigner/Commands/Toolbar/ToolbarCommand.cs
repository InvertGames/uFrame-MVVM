namespace Invert.uFrame.Editor.ElementDesigner
{
    public abstract class ToolbarCommand : DiagramCommand<ElementsDiagram>, IToolbarCommand
    {
        public virtual ToolbarPosition Position { get { return ToolbarPosition.Right; } }

        public override string CanPerform(ElementsDiagram item)
        {
            if (item == null) return "No Diagram Open";
            return null;
        }
    }
}