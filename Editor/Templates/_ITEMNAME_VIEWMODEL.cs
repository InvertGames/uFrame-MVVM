using Invert.Core.GraphDesigner;
using uFrame.Graphs;

namespace uFrame.MVVM.Templates
{
    public class _ITEMNAME_VIEWMODEL : _TEMPLATETYPE_
    {
        public override string TheType(TemplateContext context)
        {
            return context.Item.Name.AsViewModel();
        }

    }
    public class _REFNAME_VIEWMODEL : _TEMPLATETYPE_
    {
        public override string TheType(TemplateContext context)
        {
            var referenceItem = context.Item as GenericReferenceItem;
            if (referenceItem != null)
            {
                return referenceItem.SourceItemObject.Name.AsViewModel();
            }
            return string.Empty;
        }

    }
}