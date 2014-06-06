namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AutoLayoutCommand : ElementsDiagramToolbarCommand
    {
        public override string Title
        {
            get { return "Auto Layout"; }
        }

        public override void Perform(ElementsDiagram item)
        {
            item.LayoutDiagram();
            item.Refresh();
        }
    }
}