using System.Diagnostics;

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

    public class PrintPlugins : ToolbarCommand
    {
        public override string Name
        {
            get { return "Print Plugins"; }
        }

        public override void Perform(ElementsDiagram item)
        {
            foreach (var diagramPlugin in uFrameEditor.Plugins)
            {
                UnityEngine.Debug.Log(diagramPlugin.GetType().Name);
            }
            
            
        }
    }
}