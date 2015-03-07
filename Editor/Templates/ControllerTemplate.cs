using System.CodeDom;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass( MemberGeneratorLocation.Both, ClassNameFormat = uFrameFormats.CONTROLLER_FORMAT)]
public partial class ControllerTemplate : Controller, IClassTemplate<ElementNode>
{
    public TemplateContext<ElementNode> Ctx { get; set; }

    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "Controllers"); }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        if (Ctx.IsDesignerFile)
        {
            Ctx.CurrentDecleration.Attributes = MemberAttributes.Abstract;
        }
        Ctx.AddIterator("CommandMethod", _ => _.Commands.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
        Ctx.AddIterator("CommandMethodWithArg", _ => _.Commands.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName)));
        Ctx.AddIterator("InstanceProperty", _ => _.GetParentNodes().OfType<SubsystemNode>().SelectMany(p => p.Instances));
        Ctx.AddIterator("ControllerProperty", _=>_.GetParentNodes().OfType<SubsystemNode>().SelectMany(p=>p.GetContainingNodesInProject(p.Project)).OfType<ElementNode>());
    }

    public string NameAsViewModel { get { return Ctx.Data.Name.AsViewModel(); }}

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public ViewModel InstanceProperty
    {
        get
        {
            Ctx.CurrentProperty.CustomAttributes.Add(new CodeAttributeDeclaration(
                typeof (InjectAttribute).ToCodeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression(Ctx.ItemAs<InstancesReference>().Name))
                ));
            
            return null;
        }
        set
        {
            
        }
    }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public ViewModel ControllerProperty
    {
        get
        {
            var name = Ctx.Item.Name.AsController();
            Ctx.SetType(name);
            Ctx.CurrentProperty.Name = name;
            Ctx.AddAttribute("Inject");
            return null;
        }
        set
        {

        }
    }

    [TemplateMethod(MemberGeneratorLocation.DesignerFile)]
    public override void Initialize(ViewModel viewModel)
    {
        if (!Ctx.IsDesignerFile) return;
        Ctx.CurrentMethodAttribute.CallBase = Ctx.Data.BaseNode != null;
        Ctx._("this.Initialize{0}((({1})(viewModel)))",Ctx.Data.Name,NameAsViewModel);
    }

    [TemplateMethod("Create{0}", MemberGeneratorLocation.DesignerFile, false)]
    public ViewModel CreateElement()
    {
        Ctx.SetType(NameAsViewModel);
        Ctx._("return (({0})(this.Create()))", NameAsViewModel);
        return null;
    }

    [TemplateMethod( MemberGeneratorLocation.DesignerFile, false)]
    public override ViewModel CreateEmpty()
    {
        Ctx._("return new {0}(this)", NameAsViewModel);
        return null;
    }

    [TemplateMethod("Initialize{0}",MemberGeneratorLocation.Both,true)]
    public virtual void InitializeElement(ViewModel viewModel)
    {
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(NameAsViewModel);
    }

    [TemplateMethod("{0}", MemberGeneratorLocation.Both, true)]
    public virtual void CommandMethod(ViewModel viewModel)
    {
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.Data.Name + "ViewModel");
        DoTransition();
    }

    private void DoTransition()
    {
        if (Ctx.IsDesignerFile)
        {
            var transition = Ctx.Item.OutputTo<TransitionsChildItem>();
            if (transition != null)
            {
                
                var stateMachineProperty =
                    Ctx.Data.InheritedProperties.FirstOrDefault(p=>p.RelatedTypeNode is StateMachineNode);

                if (stateMachineProperty != null)
                {
                    Ctx._("viewModel.{0}.Transition(\"{1}\")", stateMachineProperty.Name.AsSubscribableProperty(),
                        transition.Name);
                }
            }
        }
    }

    [TemplateMethod("{0}", MemberGeneratorLocation.Both,true)]
    public virtual void CommandMethodWithArg(ViewModel viewModel, object arg)
    {
        CommandMethod(viewModel);
        Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
        DoTransition();

    }
}
