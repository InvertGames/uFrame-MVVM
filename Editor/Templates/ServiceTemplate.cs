using System.CodeDom;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;

[TemplateClass(MemberGeneratorLocation.Both)]
public class ServiceTemplate : ISystemService, IClassTemplate<ServiceNode>
{


    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameOnlyWithBackingField)]
    public IEventAggregator EventAggregator
    {
        get
        {
            Ctx.CurrentProperty.Name = "EventAggregator";
            Ctx.AddAttribute(typeof(InjectAttribute));

            return null;
        }
        set { }
    }
    [TemplateMethod(MemberGeneratorLocation.Both)]
    public virtual void Setup()
    {

        Ctx.TryAddNamespace("UniRx");
        if (Ctx.IsDesignerFile)
        {
            Ctx.CurrentDecleration.BaseTypes.Clear();
            if (Ctx.Data.MonoBehaviour)
            {
                Ctx.SetBaseType(typeof(MonoBehaviour));

                Ctx.TryAddNamespace("UnityEngine");

            }
            Ctx.CurrentDecleration.BaseTypes.Add(typeof(ISystemService).ToCodeReference());

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
        Ctx.TryAddNamespace("UniRx");
        if (Ctx.IsDesignerFile)
        {
            Ctx.SetType("ISystemService");
        }
        //Ctx.AddIterator("CommandMethod", _ => _.Handlers.Select(p=>p.SourceItem).OfType<CommandsChildItem>());
        //Ctx.AddIterator("CommandMethodWithArg", _ => _.Handlers.Select(p => p.SourceItem).OfType<CommandsChildItem>().Where(p => !string.IsNullOrEmpty(p.RelatedTypeName)));


        Ctx.AddIterator("OnCommandMethod",
            _ => _.Handlers.Select(p => p.SourceItemObject));


    }

    public TemplateContext<ServiceNode> Ctx { get; set; }


    //[TemplateMethod("{0}", MemberGeneratorLocation.Both, true)]
    //public virtual void CommandMethod(ViewModel viewModel)
    //{
    //    Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Item.Node.Name + "ViewModel");
    //}
    [TemplateMethod("{0}", MemberGeneratorLocation.Both, true)]
    public virtual void OnCommandMethod(ViewModelCommand data)
    {
   
        Ctx.CurrentMethod.Name = Ctx.Item.Name + "Handler";
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.ItemAs<IClassTypeNode>().ClassName);
    }


    //[TemplateMethod("{0}", MemberGeneratorLocation.Both, true)]
    //public virtual void CommandMethodWithArg(ViewModel viewModel, object arg)
    //{
    //    CommandMethod(viewModel);
    //    Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
    //}
}