using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public class PopToFilterCommand : ToolbarCommand, IDynamicOptionsCommand
    {

        public override void Perform(ElementsDiagram item)
        {

            item.Data.PopToFilter(SelectedOption.Name);
            item.Refresh(true);
        }

        public IEnumerable<ContextMenuItem> GetOptions(ElementsDiagram item)
        {

            yield return new ContextMenuItem() { Name = item.Data.SceneFlowFilter.Name, Checked = item.Data.CurrentFilter == item.Data.SceneFlowFilter };
            foreach (var filter in item.Data.FilterPath)
            {
                yield return new ContextMenuItem() {Name = filter.Name,Checked = item.Data.CurrentFilter == filter};
            }
        }

        public override ToolbarPosition Position
        {
            get { return ToolbarPosition.Left; }
        }

        public ContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get{ return MultiOptionType.Buttons; } }
    }
}