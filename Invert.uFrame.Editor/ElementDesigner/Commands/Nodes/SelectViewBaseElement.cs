using System.Collections.Generic;
using System.Linq;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class SelectViewBaseElement : EditorCommand<ViewData>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "View"; }
        }
        public override void Perform(ViewData node)
        {
            if (node == null) return;
            var view = SelectedOption.Value as ViewData;
            if (view == null)
            {
                node.BaseViewIdentifier = null;
            }
            else
            {
                node.BaseViewIdentifier = view.Identifier;    
            }
            
        }

        public override string CanPerform(ViewData node)
        {
            if (node == null) return "This operation can only be performed on a view.";
            return null;
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var view = item as ViewData;
            if (view == null) yield break;
            var element = view.ViewForElement;
            if (element == null) yield break;

            var baseViews = element.AllBaseTypes.SelectMany(
                p => view.Data.AllDiagramItems.OfType<ViewData>().Where(x => x.ViewForElement == p));
            yield return new UFContextMenuItem()
            {
                Name = "Base View/" + element.NameAsViewBase,
                Value = null
            };
            foreach (var baseView in baseViews)
            {
                yield return new UFContextMenuItem()
                {
                    Name = "Base View/" + baseView.NameAsView,
                    Value = baseView
                };
            }

        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }
}