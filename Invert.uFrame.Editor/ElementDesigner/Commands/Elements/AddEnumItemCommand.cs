using System;
using System.Diagnostics;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddEnumItemCommand : EditorCommand<EnumData>
    {
        public override void Perform(EnumData node)
        {
            node.EnumItems.Add(new EnumItem()
            {
                Name = node.Data.GetUniqueName("Item")
            });
        }

        public override string CanPerform(EnumData node)
        {
            if (node == null) return "Arg can't be null";
            return null;
        }

    }
}