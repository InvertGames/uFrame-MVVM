namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AutoLayoutCommand : ToolbarCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            item.LayoutDiagram();
            item.Refresh();
        }
    }
}