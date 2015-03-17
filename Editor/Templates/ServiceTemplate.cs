using System.CodeDom;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
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
            Ctx.AddAttribute(typeof (InjectAttribute));

            return null;
        }
        set {  }
    }
    [TemplateMethod(MemberGeneratorLocation.Both)]
    public virtual void Setup()
    {
        Ctx._comment("This is called when the controller is created");
        
        if (Ctx.IsDesignerFile)
        {
            Ctx.SetBaseType(typeof(ISystemService));
            foreach (var command in Ctx.Data.Handlers.Select(p=>p.SourceItem).OfType<CommandsChildItem>())
            {
                Ctx._("this.OnEvent<{0}Command>().Subscribe(this.{0}Handler)", command.Name);
            }
            foreach (var command in Ctx.Data.Handlers.Where(p => !(p.SourceItem is CommandsChildItem)))
            {
                Ctx._("this.OnEvent<{0}>().Subscribe(this.{0}Handler)", command.Name);
            }
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
            _ => _.Handlers.Select(p=>p.SourceItem));


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
        var c = Ctx.TypedItem;
        Ctx.CurrentMethod.Name = c.Name + "Handler";
        if (Ctx.Item is CommandsChildItem)
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Item.Name + "Command");
        }
        else
        {
            Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Item.Name);
        }
        
        //if (Ctx.IsDesignerFile)
        //{
        //    if (Ctx.Item is CommandsChildItem)
        //    {
        //        if (string.IsNullOrEmpty(c.RelatedType))
        //        {
        //            Ctx._("this.{0}(command.Sender as {1})", c.Name, c.Node.Name.AsViewModel());
        //        }
        //        else
        //        {
        //            if (Ctx.Item.OutputTo<CommandNode>() == null)
        //            {
        //                Ctx._("this.{0}(command.Sender as {1}, command.Argument)", c.Name, c.Node.Name.AsViewModel());
        //            }
        //            else
        //            {
        //                Ctx._("this.{0}(command.Sender as {1}, command)", c.Name, c.Node.Name.AsViewModel());
        //            }

        //        }
        //    }

        //}
    }


    //[TemplateMethod("{0}", MemberGeneratorLocation.Both, true)]
    //public virtual void CommandMethodWithArg(ViewModel viewModel, object arg)
    //{
    //    CommandMethod(viewModel);
    //    Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
    //}
}