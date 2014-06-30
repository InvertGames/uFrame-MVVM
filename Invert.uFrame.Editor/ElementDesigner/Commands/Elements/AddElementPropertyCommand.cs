namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddElementPropertyCommand : EditorCommand<ElementData>
    {
        public override void Perform(ElementData node)
        {
            node.Properties.Add(new ViewModelPropertyData()
            {
                Node = node,
                DefaultValue = string.Empty,
                Name = node.Data.GetUniqueName("String1"),
                Type = typeof(string)
            });
        }

        public override string CanPerform(ElementData node)
        {
            if (node == null) return "Arg can't be null";
            return null;
        }
    }
}