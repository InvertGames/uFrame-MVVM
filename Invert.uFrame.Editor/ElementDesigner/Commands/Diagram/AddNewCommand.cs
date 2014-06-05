using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AddNewCommand : ElementsDiagramToolbarCommand, IParentCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            // No implementation
        }

        public ContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get{return MultiOptionType.DropDown;} }
    }

    public abstract class ElementsDiagramToolbarCommand : ToolbarCommand<ElementsDiagram>
    {
        public override string CanPerform(ElementsDiagram item)
        {
            if (item == null) return "No Diagram Open";
            return null;
        }
    }
}