using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public class AutoLayoutCommand : ElementsDiagramToolbarCommand
    {
        public override string Title
        {
            get { return "Auto Layout"; }
        }

        public override ToolbarPosition Position
        {
            get { return ToolbarPosition.BottomRight; }
        }

        public override void Perform(ElementsDiagram node)
        {
            var x = 0f;
            var y = 20f;
            foreach (var viewModelData in node.Data.GetDiagramItems())
            {
                viewModelData.Location = new Vector2(x, y);
                x += viewModelData.Position.width + 10f;
                //y += viewModelData.Position.height + 10f;
            }
            node.Refresh();
        }
    }
}