using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public abstract class ContextMenuCommand<TFor> : DiagramCommand<TFor>
    {
        public abstract IEnumerable<ContextMenuItem> GetMenuOptions();
    }
}