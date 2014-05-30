using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IDynamicOptionsCommand
    {
        IEnumerable<ContextMenuItem> GetOptions(ElementsDiagram item);
        ContextMenuItem SelectedOption { get; set; }
        MultiOptionType OptionsType { get; }
    }
}