using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

[TemplateClass(MemberGeneratorLocation.Both)]
public class ViewComponentTemplate : IClassTemplate<ViewComponentNode>, IClassRefactorable
{
    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "ViewComponents"); }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {

        Ctx.SetBaseType(InvertApplication.FindType(typeof(ViewComponent).FullName));

        Ctx.AddIterator("ExecuteCommand", _ => _.View.Element.InheritedCommandsWithLocal.Where(p => string.IsNullOrEmpty(p.RelatedTypeName)));
        Ctx.AddIterator("ExecuteCommandOverload", _ => _.View.Element.InheritedCommandsWithLocal);
        Ctx.AddIterator("ExecuteCommandWithArg", _ => _.View.Element.InheritedCommandsWithLocal.Where(p => !string.IsNullOrEmpty(p.RelatedTypeName) && p.OutputCommand == null));
        
    }

    public TemplateContext<ViewComponentNode> Ctx { get; set; }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile)]
    public virtual object Player
    {
        get
        {
            Ctx.CurrentProperty.Name = Ctx.Data.View.Element.Name;
            Ctx.CurrentProperty.Type = Ctx.Data.View.Element.Name.AsViewModel().ToCodeReference();
            Ctx._("return ({0})this.View.ViewModelObject", Ctx.Data.View.Element.Name.AsViewModel());
            return null;
        }
    }

    [TemplateMethod("Execute{0}", MemberGeneratorLocation.DesignerFile, false, AutoFill = AutoFillType.NameOnly)]
    public void ExecuteCommand()
    {
        Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0} }})", Ctx.Data.View.Element.Name, Ctx.Item.Name);
        //Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Element.Name, Ctx.Item.Name);
    }
    [TemplateMethod("Execute{0}", MemberGeneratorLocation.DesignerFile, false, AutoFill = AutoFillType.NameOnly)]
    public void ExecuteCommandOverload(ViewModelCommand command)
    {
        Ctx.CurrentMethod.Parameters[0].Type = (Ctx.Item.Name + "Command").ToCodeReference();
        Ctx._("command.Sender = {0}", Ctx.Data.View.Element.Name);
        Ctx._("{0}.{1}.OnNext(command)", Ctx.Data.View.Element.Name, Ctx.Item.Name);
        //Ctx._("this.ExecuteCommand({0}.{1})", Ctx.Data.Element.Name, Ctx.Item.Name);
    }
    [TemplateMethod("Execute{0}", MemberGeneratorLocation.DesignerFile, false, AutoFill = AutoFillType.NameOnly)]
    public void ExecuteCommandWithArg(object arg)
    {
        Ctx.CurrentMethod.Parameters[0].Type = new CodeTypeReference(Ctx.TypedItem.RelatedTypeName);
        Ctx._("{0}.{1}.OnNext(new {1}Command() {{ Sender = {0}, Argument = arg }})", Ctx.Data.View.Element.Name, Ctx.Item.Name);
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

