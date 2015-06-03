using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile)]
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

        }

        public TemplateContext<StateMachineNode> Ctx { get; set; }

        [GenerateConstructor("vm", "propertyName")]
        public void StateMachineConstructor(ViewModel vm, string propertyName)
        {

        }

        [GenerateConstructor("null", "string.Empty")]
        public void StateMachineConstructor()
        {

        }


        [GenerateProperty]
        public override Invert.StateMachine.State StartState
        {
            get
            {
                Ctx._("return this.{0}", Ctx.Data.StartStateOutputSlot.OutputTo<StateNode>().Name);
                return null;
            }
        }

        public IEnumerable<TransitionsChildItem> DistinctTransitions
        {
            get { return Ctx.Data.Transitions.Distinct(); }
        }

        public IEnumerable<StateNode> DistinctStates
        {
            get { return Ctx.Data.States.Distinct(); }
        }

        [ForEach("DistinctTransitions"), GenerateProperty, WithField]
        public virtual StateMachineTrigger _TriggerName_
        {
            get
            {
                Ctx._if("this.{0} == null", Ctx.Item.Name.AsField())
                    .TrueStatements
                    ._("this.{0} = new StateMachineTrigger(this , \"{1}\")", Ctx.Item.Name.AsField(), Ctx.Item.Name);

//            Ctx._("return this.{0}", Ctx.Item.Name.AsField());
                return null;
            }
        }


        [ForEach("DistinctStates"), GenerateProperty, WithField]
        public virtual State _StateName_
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

        [GenerateMethod]
        public override void Compose(List<State> states)
        {
            //base.Compose(states);
            foreach (var state in Ctx.Data.States)
            {
                foreach (var transition in state.StateTransitions)
                {
                    var to = transition.OutputTo<StateNode>();
                    if (to == null) continue;

                    Ctx._("{0}.{1} = new StateTransition(\"{1}\", {0}, {2})", state.Name, transition.Name, to.Name);
                    Ctx._("Transitions.Add({0}.{1})", state.Name, transition.Name);
                }
                foreach (var transition in state.StateTransitions)
                {
                    //var to = transition.OutputTo<StateNode>();
                    Ctx._("{0}.AddTrigger({1}, {0}.{1})", state.Name, transition.Name);
                }
                Ctx._("{0}.StateMachine = this", state.Name);
                Ctx._("states.Add({0})", state.Name);

            }
        }


    }
}