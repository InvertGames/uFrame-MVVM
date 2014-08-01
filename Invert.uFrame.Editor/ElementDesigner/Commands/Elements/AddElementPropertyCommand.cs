using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddElementPropertyCommand : EditorCommand<ElementData>
    {
        public override void Perform(ElementData node)
        {
            node.Properties.Add(new ViewModelPropertyData()
            {
                Node = node,
                DefaultValue = string.Empty,
                Name = node.Data.GetUniqueName("String1"),
                Type = typeof(string)
            });
        }

        public override string CanPerform(ElementData node)
        {
            if (node == null) return "Arg can't be null";
            return null;
        }
    }
    public class AddViewPropertyCommand : EditorCommand<ViewData>
    {
        public override void Perform(ViewData node)
        {
            var quickFind = new MemberInfo[]
            {
                typeof (Transform).GetMember("position").First(),
                typeof (Transform).GetMember("localPosition").First(),
                typeof (Transform).GetMember("rotation").First(),
                typeof (Transform).GetMember("localRotation").First(),
                typeof (Transform).GetMember("localScale").First(),
                typeof (Rigidbody).GetMember("velocity").First(),
                typeof (Rigidbody).GetMember("angularVelocity").First(),
                typeof (Rigidbody).GetMember("drag").First(),
                typeof (Rigidbody).GetMember("angularDrag").First(),
                typeof (Rigidbody).GetMember("mass").First(),
                typeof (Rigidbody).GetMember("useGravity").First(),


            };
            uFrameComponentSearchWindow.ShowWindow((w,m) =>
            {
                node.Properties.Add(new ViewPropertyData()
                {
                    Node = node,
                    Name = m.Name,
                    ComponentProperty = m.Name,
                    ComponentTypeName = m.DeclaringType.FullName
                });

            }, (w,m) =>
            {
                node.Properties.RemoveAll(p=>p.MemberInfo == m);
            },node.Properties.Select(p=>p.MemberInfo).ToArray(),quickFind);
           
        }

        public override string CanPerform(ViewData node)
        {
            if (node == null) return "Arg can't be null";
            return null;
        }
    }

    public class AddBindingCommand : EditorCommand<ViewData>
    {
        public override void Perform(ViewData node)
        {
            AddBindingWindow.Init("Add Binding", node);
        }

        public override string CanPerform(ViewData node)
        {
            return null;
        }
    }
}