using System.Collections.Generic;
using Invert.uFrame.Editor.ElementDesigner.Commands;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AddNewCommand : ElementsDiagramToolbarCommand, IParentCommand,IDiagramContextCommand
    {
        public override string Title
        {
            get { return "Add New"; }
        }

        public override void Perform(ElementsDiagram node)
        {
            // No implementation
        }
    }

    public abstract class ElementsDiagramToolbarCommand : ToolbarCommand<ElementsDiagram>
    {
        public override string CanPerform(ElementsDiagram node)
        {
            if (node == null) return "No Diagram Open";
            return null;
        }
    }
}