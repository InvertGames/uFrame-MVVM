using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddNewViewComponentCommand : AddItemCommand<ViewComponentData>
    {
        public override void Perform(ElementsDiagram item)
        {
            var data = new ViewComponentData()
            {
                Data = item.Data,
                Name = item.Data.GetUniqueName("NewViewComponent"),
                Location = new Vector2(15, 15)
            };
            item.Data.ViewComponents.Add(data);
            data.Location = item.LastMouseDownPosition;
        }
    }
}