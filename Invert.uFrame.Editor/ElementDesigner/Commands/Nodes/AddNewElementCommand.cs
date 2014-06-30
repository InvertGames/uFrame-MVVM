namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddNewElementCommand : AddItemCommand<ElementData>
    {
        public override string Title
        {
            get { return "Add New Element"; }
        }

        public override void Perform(ElementsDiagram node)
        {
            var data = new ElementData
            {
                Data = node.Data,
                Name = node.Data.GetUniqueName("NewElement"),
                //BaseTypeName = typeof(ViewModel).FullName,
                Dirty = true
            };
            data.Location = node.LastMouseDownPosition;
            data.Filter.Locations[data] = data.Location;
            node.Data.AddNode(data);
        }
    }

}