using System;
using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public abstract class EditorCommand : IEditorCommand
    {
        private List<IEditorCommand> _hooks;
        private string _title;


        public abstract Type For { get; }

        public void Execute(object item)
        {
            Perform(item);
        }

        public abstract void Perform(object arg);

        public List<IEditorCommand> Hooks
        {
            get { return _hooks ?? (_hooks = new List<IEditorCommand>()); }
            set { _hooks = value; }
        }

        public virtual string Name
        {
            get { return this.GetType().Name.Replace("Command",""); }
        }

        public virtual string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    return Name;
                }
                return _title;
            }
            set { _title = value; }
        }

        public string CanExecute(object arg)
        {
            return null;
        }

        public virtual decimal Order { get { return 0; } }
        public bool ShowAsDiabled { get { return false; } }
        public virtual string Group { get { return "Default"; }}
        public abstract string CanPerform(object arg);
        public virtual string Path { get { return Name; } }

        public virtual bool IsChecked(object arg)
        {
            return false;
        }
    }
    
}