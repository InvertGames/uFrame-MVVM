using System.CodeDom;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using uFrame.Graphs;
using UnityEngine;

[TemplateClass("Controllers", MemberGeneratorLocation.Both, ClassNameFormat = uFrameFormats.CONTROLLER_FORMAT)]
public class ControllerTemplate : Controller, IClassTemplate<ElementNode>
{
    public TemplateContext<ElementNode> Ctx { get; set; }

    public void TemplateSetup()
    {
        if (Ctx.IsDesignerFile)
        {
            Ctx.CurrentDecleration.Attributes = MemberAttributes.Abstract;
        }
        Ctx.AddIterator("CommandMethod", _ => _.Commands.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
        Ctx.AddIterator("CommandMethodWithArg", _ => _.Commands.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName)));
    }

    public string NameAsViewModel { get { return Ctx.Data.Name.AsViewModel(); }}

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
        //Ctx.SetType(NameAsViewModel);
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
            Debug.Log("YUPYUPYUP");
            var transition = Ctx.Item.OutputTo<StateMachineTransition>();
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

//public class MyGeneratorsPlugin : DiagramPlugin
//{
//    public override void Initialize(uFrameContainer container)
//    {
//        var uFramePlugin = container.Resolve<GameFramework>();
//        uFramePlugin.State.AddCodeTemplate<StateViewComponent>();
//    }
//}
//[TemplateClass("StateComponents", MemberGeneratorLocation.Both, ClassNameFormat = "{0}StateComponent")]
//public class StateViewComponent : ViewComponent, IClassTemplate<StateNode>
//{
//    #region Template Stuff
//    public void TemplateSetup()
//    {
        
//    }

//    public TemplateContext<StateNode> Ctx { get; set; }
//    #endregion

//    public override void Bind(ViewBase view)
//    {
//        base.Bind(view);
//        Ctx._comment("This comment will be inside the generated Bind method");

//    }
//}

