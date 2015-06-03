using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile)]
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

        }

        public TemplateContext<StateNode> Ctx { get; set; }

        //[GenerateProperty(TemplateLocation.DesignerFile, AutoFill = AutoFillType.NameOnlyWithBackingField)]
        [ForEach("StateTransitions"), GenerateProperty("{0}"), WithField]
        public StateTransition TransitionProperty { get; set; }

        [GenerateProperty]
        public override string Name
        {
            get
            {
                Ctx._("return \"{0}\"", Ctx.Data.Name);
                return null;
            }
        }

        //[GenerateMethod(TemplateLocation.DesignerFile, AutoFill = AutoFillType.NameOnly, NameFormat = "{0}Transition")]
        [ForEach("StateTransitions"), GenerateMethod(CallBase = false), WithNameFormat("{0}Transition")]
        public void TransitionMethod()
        {
            Ctx._("this.Transition(this.{0})", Ctx.Item.Name);
        }

    }
}
