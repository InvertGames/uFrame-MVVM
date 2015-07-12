using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using uFrame.Kernel;
using uFrame.MVVM;
using UnityEngine;
using UnityEngine.EventSystems;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.Both)]
    [RequiresNamespace("UnityEngine")]
    [RequiresNamespace("UniRx")]
    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.Kernel")]
    [RequiresNamespace("uFrame.IOC")]
    [RequiresNamespace("uFrame.MVVM")]
    [NamespacesFromItems]
    [ForceBaseType(typeof(SystemServiceMonoBehavior))]
    public partial class ServiceTemplate : IClassTemplate<ServiceNode>, IClassRefactorable
    {

        [GenerateMethod(TemplateLocation.Both, true), AsOverride]
        public void Setup()
        {
            Ctx.CurrentMethod.invoke_base();
            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("<summary>", true));
            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("This method is invoked whenever the kernel is loading.", true));
            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("Since the kernel lives throughout the entire lifecycle of the game, this will only be invoked once.", true));
            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("</summary>", true));
            //Ctx.CurrentMethod.Attributes |= MemberAttributes.Override;
            if (Ctx.IsDesignerFile)
            {
                foreach (var command in Ctx.Data.Handlers.Select(p => p.SourceItemObject).OfType<IClassTypeNode>())
                {
                    Ctx._("this.OnEvent<{0}>().Subscribe(this.{1}Handler)", command.ClassName, command.Name);
                }

            }
            else
            {
                Ctx.CurrentMethod.invoke_base();
                Ctx._comment("Use the line below to subscribe to events.");
                Ctx._comment("this.OnEvent<MyEvent>().Subscribe(myEventInstance=>{{  TODO }});");
            }
        }

        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "Services"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {

        }

        public TemplateContext<ServiceNode> Ctx { get; set; }

        public IEnumerable<IDiagramNodeItem> Handlers
        {
            get { return Ctx.Data.Handlers.Select(p => p.SourceItemObject); }
        }

        [ForEach("Handlers"), GenerateMethod(CallBase = true), Inside(TemplateLocation.Both)]
        public virtual void _Name_Handler(ViewModelCommand data)
        {

            Ctx.CurrentMethod.Name = Ctx.Item.Name + "Handler";
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.ItemAs<IClassTypeNode>().ClassName);
            Ctx._comment("Process the commands information.  Also, you can publish new events by using the line below.");
            Ctx._comment("this.Publish(new AnotherEvent())");

            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("<summary>", true));
            Ctx.CurrentMethod.Comments.Add(
                    new CodeCommentStatement(string.Format("This method is executed when using this.Publish(new {0}())",
                        Ctx.ItemAs<IClassTypeNode>().ClassName)));
            Ctx.CurrentMethod.Comments.Add(new CodeCommentStatement("</summary>", true));

        }

        public IEnumerable<string> ClassNameFormats
        {
            get
            {
                yield return "{0}";
                yield return "{0}Base";
            }
        }
    }
}
