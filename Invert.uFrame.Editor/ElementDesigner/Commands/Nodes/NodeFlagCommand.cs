namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class NodeFlagCommand<T> : EditorCommand<T>, IDiagramNodeCommand where T :DiagramNode
    {
        private string _title;

        public NodeFlagCommand(string flagName,string title = null)
        {
            _title = title;
            FlagName = flagName;
        }

        public override string Group
        {
            get { return "Flags"; }
        }

        public override string Name
        {
            get { return string.IsNullOrEmpty(_title) ? FlagName : _title; }
        }

        //public override string Path
        //{
        //    get { return  Name; }
        //}

        public string FlagName { get; set; }
        
        public override bool IsChecked(T node)
        {
            return node[FlagName];
        }

        public override void Perform(T node)
        {
            node[FlagName] = !node[FlagName];
        }

        public override string CanPerform(T node)
        {
            if (node == null) return "Node is null. Can't perform flag set command.";
            return null;
        }
    }
}