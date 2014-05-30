using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IDiagramCommand : IContextMenuItemCommand
    {
        Type For { get; }
        void Execute(object item);
        List<CommandHook> Hooks { get; }
        string Name { get; }
        string Title { get; set; }
        string CanPerform(object arg);
    }

    public abstract class DiagramCommand<TFor> : DiagramCommand
    {
        public override Type For
        {
            get { return typeof(TFor); }
        }

        public sealed override void Perform(object item)
        {
            Perform((TFor)item);
        }

        public abstract void Perform(TFor item);

        public override string CanPerform(object arg)
        {
            return CanPerform((TFor) arg);
        }

        public abstract string CanPerform(TFor item);

    }
}