using System.CodeDom;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using uFrame.Graphs;

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

    //[TemplateMethod("Create{0}", MemberGeneratorLocation.DesignerFile, false)]
    //public ViewModel CreateElementWithProperties()
    //{
    //    foreach (var item in Ctx.Data.Properties)
    //    {
    //        Ctx.CurrentMethod.Parameters.Add(
    //            new CodeParameterDeclarationExpression(item.RelatedTypeName.ToCodeReference(), item.Name));
    //    }
    //    Ctx.SetType(NameAsViewModel);
    //    Ctx._("return (({0})(this.Create()))", NameAsViewModel);
    //    return null;
    //}

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

    }

    [TemplateMethod("{0}", MemberGeneratorLocation.Both,true)]
    public virtual void CommandMethodWithArg(ViewModel viewModel, object arg)
    {
        CommandMethod(viewModel);
        Ctx.CurrentMethod.Parameters[1].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);


    }
}