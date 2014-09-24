using System;
using System.Collections.Generic;
using UniRx;

namespace Invert.StateMachine
{
    public abstract class State : IObserver<StateMachineTrigger>
    {
        public abstract string Name { get; }

        public StateMachine StateMachine { get; set; }

        private Dictionary<StateMachineTrigger, StateTransition> _triggers;

        public void Transition(StateTransition transition)
        {
            StateMachine.Transition(transition);
        }

        public override string ToString()
        {
            return Name ?? this.GetType().Name;
        }

        private void Compose()
        {
            
       
        }



        public virtual void OnEnter(State previousState)
        {

            //if (previousState != null && previousState != this)
            //    foreach (var trigger in Triggers)
            //    {
            //        if (trigger.Key.Calculator == null) continue;
            //        if (trigger.Key.Calculator(this.StateMachine.Owner))
            //        {
            //            StateMachine.Transition(trigger.Value);
            //        }
            //    }
        }

        public virtual void OnExit(State nextState)
        {

        }

        public void OnEntering(State currentState)
        {
            
        }

        public Dictionary<StateMachineTrigger, StateTransition> Triggers
        {
            get { return _triggers ?? (_triggers = new Dictionary<StateMachineTrigger, StateTransition>()); }
            set { _triggers = value; }
        }


        public virtual void AddTrigger(StateMachineTrigger trigger,StateTransition transition)
        {
            Triggers.Add(trigger,transition);
        }

        

        public void Trigger(StateMachineTrigger transition)
        {
            OnNext(transition);
        }
        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(StateMachineTrigger value)
        {
            if (this.Triggers.ContainsKey(value))
            {
                Transition(Triggers[value]);
            }
        }
    }
}