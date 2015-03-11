using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;

[TemplateClass(MemberGeneratorLocation.DesignerFile)]
public class StateTemplate : Invert.StateMachine.State, IClassTemplate<StateNode>
{
    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "Machines"); }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        Ctx.TryAddNamespace("Invert.StateMachine");

        Ctx.AddIterator("TransitionProperty",_=>_.StateTransitions);
        Ctx.AddIterator("TransitionInvoker", _ => _.StateTransitions);
    }

    public TemplateContext<StateNode> Ctx { get; set; }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnlyWithBackingField)]
    public StateTransition TransitionProperty { get; set; }

    [TemplateProperty]
    public override string Name
    {
        get
        {
            Ctx._("return \"{0}\"",Ctx.Data.Name);
            return "Idle";
        }
    }

    [TemplateMethod(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "{0}Transition")]
    public void TransitionInvoker()
    {
        Ctx._("this.Transition(this.{0})",Ctx.Item.Name);
    }

}