using System;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public abstract class ContextDiagramCommand<TFor,TCommand> : DiagramCommand<TFor>, IContextMenuItemCommand
    {
        //public string Path { get; private set; }
        //public bool Checked { get; set; }

        public Type ContextItemFor
        {
            get { return typeof (TCommand); }
        }
    }
}