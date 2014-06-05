using System.Diagnostics;
using System.Linq;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public class SaveCommand : ElementsDiagramToolbarCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            item.Repository.Save();
            item.DeselectAll();
            item.Refresh();
        }
    }

    public class PrintPlugins : ElementsDiagramToolbarCommand
    {
        public override string Name
        {
            get { return "Print Plugins"; }
        }

        public override void Perform(ElementsDiagram item)
        {
            foreach (var diagramPlugin in uFrameEditor.GetAllCodeGenerators(item.Data))
            {
                UnityEngine.Debug.Log((diagramPlugin.IsDesignerFile ? "Designer File" : "Editable File" ) + 
                    diagramPlugin.GetType().Name + diagramPlugin.Filename );
            }
        }
    }
}