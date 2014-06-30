using System.Text;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public abstract class AddItemCommand<TType> : AddNewCommand, IDiagramContextCommand
    {
        public override string CanPerform(ElementsDiagram node)
        {
            if (node == null) return "Diagram must be loaded first.";

            if (!node.Data.CurrentFilter.IsAllowed(null, typeof(TType)))
                return "Item is not allowed in this part of the diagram.";

            return null;
        }

        public override string Path
        {
            get { return  "Add New/" + Title; }
        }
    }

    //public class DeleteCommand : EditorCommand<ISelectable>, IDiagramNodeCommand, IDiagramNodeItemCommand
    //{

    //}
}
