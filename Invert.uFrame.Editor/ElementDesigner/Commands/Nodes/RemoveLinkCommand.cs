using System.Collections.Generic;
using System.Linq;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class RemoveLinkCommand : EditorCommand<IDiagramNode>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "Default"; }
        }

        public override decimal Order
        {
            get { return -2; }
        }

        public override void Perform(IDiagramNode node)
        {
            var link = SelectedOption.Value as IDiagramLink;

            if (link != null) 
                link.Source.RemoveLink(node);
        }

        public override string CanPerform(IDiagramNode node)
        {
            return null;
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var diagramItem = item as IDiagramNode;
            if (diagramItem == null) yield break;
            var links = diagramItem.Data.Links.Where(p => p.Target == item);
            foreach (var link in links)
            {
                yield return new UFContextMenuItem()
                {
                    Checked = true,
                    Name = "Remove Link From/" + link.Source.Label,
                    Value = link
                };
            }

        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }
}