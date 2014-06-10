using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddElementCollectionCommand : EditorCommand<ElementData>
    {
        public override void Perform(ElementData node)
        {
            node.Collections.Add(new ViewModelCollectionData()
            {
                Name = node.Data.GetUniqueName("NewCollection"),
                ItemType = typeof(string)
            });
        }

        public override string CanPerform(ElementData node)
        {
            if (node == null) return "Arg can't be null";
            return null;
        }
    }
}