namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AddNewCommand : ToolbarCommand, IParentCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            // No implementation
        }

        public ContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get{return MultiOptionType.DropDown;} }
    }
}