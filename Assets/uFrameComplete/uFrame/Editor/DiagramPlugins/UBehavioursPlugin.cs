using Invert.uFrame.Editor.ElementDesigner.Data;
using UnityEditor;

namespace Assets.uFrameComplete.uFrame.Editor.DiagramPlugins
{
    public class UBehavioursPlugin : DiagramPlugin
    {
        public override IElementDrawer GetDrawer(ElementsDiagram diagram, IDiagramItem data)
        {
            if (data is ViewData)
            {
                return new UBehavioursViewDrawer(data as ViewData,diagram);
            }
            return null;
        }

        public override void OnAddContextItems(ElementsDiagram diagram, GenericMenu menu)
        {
            
        }
    }
}