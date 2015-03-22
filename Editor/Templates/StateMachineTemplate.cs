using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

[TemplateClass(MemberGeneratorLocation.DesignerFile)]
public class StateMachineTemplate : Invert.StateMachine.StateMachine, IClassTemplate<StateMachineNode>
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
        Ctx.AddIterator("TriggerProperty", _ => _.Transitions.Distinct());
        Ctx.AddIterator("StateProperty", _ => _.States.Distinct());
    }

    public TemplateContext<StateMachineNode> Ctx { get; set; }

    [TemplateConstructor(MemberGeneratorLocation.DesignerFile,"vm","propertyName")]
    public void StateMachineConstructor(ViewModel vm, string propertyName)
    {

    }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile)]
    public override Invert.StateMachine.State StartState
    {
        get
        {
            Ctx._("return this.{0}",Ctx.Data.StartStateOutputSlot.OutputTo<StateNode>().Name);
            return null;
        }
    }
 
    [TemplateProperty(MemberGeneratorLocation.DesignerFile,AutoFill = AutoFillType.NameOnlyWithBackingField)]
    public virtual StateMachineTrigger TriggerProperty
    {
        get
        {
            Ctx._if("this.{0} == null", Ctx.Item.Name.AsField())
                .TrueStatements
                ._("this.{0} = new StateMachineTrigger(this , \"{1}\")",Ctx.Item.Name.AsField(),Ctx.Item.Name);

//            Ctx._("return this.{0}", Ctx.Item.Name.AsField());
            return null;
        }
    }


    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFill = AutoFillType.NameOnlyWithBackingField)]
    public virtual State StateProperty
    {
        get
        {
            Ctx.SetType(Ctx.Item.Name);
            Ctx._if("this.{0} == null", Ctx.Item.Name.AsField())
             .TrueStatements
             ._("this.{0} = new {1}()", Ctx.Item.Name.AsField(), Ctx.Item.Name);

//            Ctx._("return this.{0}", Ctx.Item.Name.AsField());
            return null;
        }
    }

    [TemplateMethod(MemberGeneratorLocation.DesignerFile)]
    public override void Compose(List<State> states)
    {
        //base.Compose(states);
        foreach (var state in Ctx.Data.States)
        {
            foreach (var transition in state.StateTransitions)
            {
                var to = transition.OutputTo<StateNode>();
                if (to == null) continue;
               
                Ctx._("{0}.{1} = new StateTransition(\"{1}\", {0}, {2})",state.Name,transition.Name,to.Name);
                Ctx._("Transitions.Add({0}.{1})", state.Name, transition.Name); 
            }
            foreach (var transition in state.StateTransitions)
            {
                //var to = transition.OutputTo<StateNode>();
                Ctx._("{0}.AddTrigger({1}, {0}.{1})",state.Name,transition.Name);
            }
            Ctx._("{0}.StateMachine = this",state.Name);
            Ctx._("states.Add({0})",state.Name);

        }
        //this.Idle.StateMachine = this;
        //Idle.BeginFiring = new StateTransition("BeginFiring", Idle, Firing);
        //Idle.OnReload = new StateTransition("OnReload", Idle, Reloading);
        //Idle.OnEmpty = new StateTransition("OnEmpty", Idle, Empty);
        //Idle.AddTrigger(BeginFiring, Idle.BeginFiring);
        //Idle.AddTrigger(OnReload, Idle.OnReload);
        //Idle.AddTrigger(OnEmpty, Idle.OnEmpty);
        //states.Add(Idle);
        //this.Firing.StateMachine = this;
        //Firing.EndFiring = new StateTransition("EndFiring", Firing, Idle);
        //Firing.AddTrigger(EndFiring, Firing.EndFiring);
        //states.Add(Firing);
        //this.Reloading.StateMachine = this;
        //Reloading.FinishedReloading = new StateTransition("FinishedReloading", Reloading, Idle);
        //Reloading.AddTrigger(FinishedReloading, Reloading.FinishedReloading);
        //states.Add(Reloading);
        //this.Empty.StateMachine = this;
        //Empty.OnReload = new StateTransition("OnReload", Empty, Reloading);
        //Empty.AddTrigger(OnReload, Empty.OnReload);
        //states.Add(Empty);
    }

  
}