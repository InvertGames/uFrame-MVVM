using System;
using System.Collections.Generic;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IDynamicOptionsCommand
    {
        Type For { get; }
        IEnumerable<ContextMenuItem> GetOptions(object item);
        ContextMenuItem SelectedOption { get; set; }
        MultiOptionType OptionsType { get; }
    }
}