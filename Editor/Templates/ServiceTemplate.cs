using System.CodeDom;
using System.Collections;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;

[TemplateClass(MemberGeneratorLocation.Both)]
public class ServiceTemplate :  IClassTemplate<ServiceNode>
{

    [TemplateMethod(MemberGeneratorLocation.Both)]
    public void Setup()
    {
        Ctx.TryAddNamespace("UniRx");
        if (Ctx.IsDesignerFile)
        {
            foreach (var command in Ctx.Data.Handlers.Select(p => p.SourceItemObject).OfType<IClassTypeNode>())
            {
                Ctx._("this.OnEvent<{0}>().Subscribe(this.{1}Handler)", command.ClassName, command.Name);
            }
            //foreach (var command in Ctx.Data.Handlers.Where(p => !(p.SourceItem is CommandsChildItem)))
            //{
            //    Ctx._("this.OnEvent<{0}>().Subscribe(this.{0}Handler)", command.Name);
            //}
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
        Ctx.TryAddNamespace("UnityEngine");
        Ctx.TryAddNamespace("UniRx");
        
        if (Ctx.IsDesignerFile)
        {
            Ctx.SetBaseType(typeof(SystemServiceMonoBehavior));
        }

        foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
        {
            var type = InvertApplication.FindTypeByName(property.RelatedTypeName);
            if (type == null) continue;

            Ctx.TryAddNamespace(type.Namespace);
        }

        Ctx.AddIterator("OnCommandMethod",
            _ => _.Handlers.Select(p => p.SourceItemObject));


    }

    public TemplateContext<ServiceNode> Ctx { get; set; }


    [TemplateMethod("{0}", MemberGeneratorLocation.Both, true)]
    public virtual void OnCommandMethod(ViewModelCommand data)
    {
   
        Ctx.CurrentMethod.Name = Ctx.Item.Name + "Handler";
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.ItemAs<IClassTypeNode>().ClassName);
    }

}