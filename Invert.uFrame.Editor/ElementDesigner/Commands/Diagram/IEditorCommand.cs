using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IEditorCommand : IContextMenuItemCommand
    {
        Type For { get; }
        void Execute(object item);
        List<IEditorCommand> Hooks { get; }
        string Name { get; }
        string Title { get; set; }
        decimal Order { get; }
        bool ShowAsDiabled { get; }
        string Group { get;  }
        string CanPerform(object arg);

        
    }

    public class HookCommand : EditorCommand
    {
        public HookCommand(Action action)
        {
            Action = action;
        }

        public override Type For
        {
            get { return typeof (ElementsDiagram); }
        }

        public Action Action { get; set; }

        public HookCommand()
        {
        }

        public override string CanPerform(object arg)
        {
            return null;
        }

        public override void Perform(object arg)
        {
            Action();
        }
    }
    public class SimpleEditorCommand<TFor> : EditorCommand<TFor>
    {

        public SimpleEditorCommand(Action<TFor> performAction)
        {
            PerformAction = performAction;
        }

        public Action<TFor> PerformAction { get; set; }

        public override string CanPerform(TFor arg)
        {
            return null;
        }

        public override void Perform(TFor arg)
        {
            this.PerformAction(arg);
        }
    }
    public abstract class EditorCommand<TFor> : EditorCommand
    {
        public override Type For
        {
            get { return typeof(TFor); }
        }

        public sealed override void Perform(object item)
        {
            Perform((TFor)item);
        }

        public abstract void Perform(TFor node);

        public override string CanPerform(object arg)
        {
            return CanPerform((TFor) arg);
        }

        public sealed override bool IsChecked(object arg)
        {
            if (arg == null) return false;
            return IsChecked((TFor)arg);
        }

        public virtual bool IsChecked(TFor arg)
        {
            return false;
        }

        public abstract string CanPerform(TFor node);

    }
}