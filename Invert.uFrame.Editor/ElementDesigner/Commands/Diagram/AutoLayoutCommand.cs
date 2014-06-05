namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AutoLayoutCommand : ElementsDiagramToolbarCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            item.LayoutDiagram();
            item.Refresh();
        }
    }
}