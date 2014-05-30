namespace Invert.uFrame.Editor.ElementDesigner
{
    public class SaveCommand : ToolbarCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            item.Repository.Save();
            item.DeselectAll();
            item.Refresh();
        }
    }
}