using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public abstract class AddItemCommand<TType> : DiagramCommand<ElementsDiagram>, IContextMenuItemCommand
    {
        public override string CanPerform(ElementsDiagram item)
        {
            if (item == null) return "Diagram must be loaded first.";

            if (!item.Data.CurrentFilter.IsAllowed(null, typeof(TType)))
                return "Item is not allowed in this part of the diagram.";

            return null;
        }

        public override string Path
        {
            get { return "Add Item/" + Title; }
        }


    }

    public class RenameCommand : DiagramCommand<IDiagramItem>
    {
        public override void Perform(IDiagramItem item)
        {
            item.BeginEditing();
        }

        public override string CanPerform(IDiagramItem item)
        {
            if (item == null) return "Invalid argument";
            return null;
        }
    }

    public class DeleteCommand : DiagramCommand<IDiagramItem>
    {
        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
    public class HideCommand : DiagramCommand<IDiagramItem>
    {
        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
    public class OpenSceneManagerFileCommand : DiagramCommand<IDiagramItem>
    {
        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
    public class OpenControllerFileCommand : DiagramCommand<IDiagramItem>
    {
        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
    public class OpenViewFileCommand : DiagramCommand<IDiagramItem>
    {
        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
    public class OpenViewComponentFileCommand : DiagramCommand<IDiagramItem>
    {

        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
    public class OpenViewModelFileCommand : DiagramCommand<IDiagramItem>
    {

        public override void Perform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }

        public override string CanPerform(IDiagramItem item)
        {
            throw new NotImplementedException();
        }
    }
}
