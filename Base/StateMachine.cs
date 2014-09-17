using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;

namespace Invert.StateMachine
{
    public abstract class State 
    {
        public abstract string Name { get; }

        public StateMachine StateMachine { get; set; }

        private List<StateTransition> _transitions;
        private Dictionary<Computed<bool>, StateTransition> _triggers;


        public void Transition(StateTransition transition)
        {
            StateMachine.Transition(transition);
        }

        public override string ToString()
        {
            return Name ?? this.GetType().Name;
        }

        public List<StateTransition> Transitions
        {
            get
            {
                if (_transitions != null) return _transitions;
                Compose();
                return _transitions;
            }
            set { _transitions = value; }
        }

        private void Compose()
        {
            
            _transitions = new List<StateTransition>();
            Compose(_transitions);
        }


        public virtual void Compose(List<StateTransition> action)
        {
           
        }

        public virtual void OnEnter(State previousState)
        {
            if (previousState != null && previousState != this)
            foreach (var trigger in Triggers)
            {
                if (trigger.Key.Calculator == null) continue;
                if (trigger.Key.Calculator(this.StateMachine.Owner))
                {
                    StateMachine.Transition(trigger.Value);
                }
            }
        }

        public virtual void OnExit(State nextState)
        {

        }

        public void OnEntering(State currentState)
        {
            
        }

        public virtual void AddTrigger(Computed<bool> property, StateTransition transition)
        {
            property.Subscribe((v) =>
            {
                if (v) Transition(transition);
            },false);
            Triggers.Add(property,transition);
        }

        protected Dictionary<Computed<bool>, StateTransition> Triggers
        {
            get { return _triggers ?? (_triggers = new Dictionary<Computed<bool>, StateTransition>()); }
            set { _triggers = value; }
        }
    }

    public class StateMachine : P<State>
    {

        private List<State> _states;
 

        public StateMachine(ViewModel owner, string propertyName) : base(owner, propertyName)
        {
            Compose();
        }

        private void Compose()
        {
            _states = new List<State>();
            _states.Clear();
            Compose(_states);
            Transitions = _states.SelectMany(p => p.Transitions).ToArray();
            CurrentState = StartState;
            Value = StartState;
        }
        public List<State> States
        {
            get
            {
             
                return _states;
            }
            set { _states = value; }
        }

        public virtual State StartState
        {
            get { return States.FirstOrDefault(); }
        }

        protected override void OnPropertyChanged(string value)
        {
            if (LastValue != null)
                LastValue.OnExit(CurrentState);

            base.OnPropertyChanged(value);
            
            if (Value != null)
                Value.OnEnter(LastValue);
        }

        public State CurrentState
        {
            get { return Value; }
            set { Value = value; }
        }
        
        public virtual void Compose(List<State> states)
        {
            
        }

        public virtual void Awake()
        {
            var startState = StartState;
            if (startState != null)
            {
                CurrentState = startState;
            }
        }


        public StateTransition[] Transitions { get; set; }

        public void Transition(string name)
        {
            StateTransition transition = null;
            for (int index = 0; index < Transitions.Length; index++)
            {
                StateTransition p = Transitions[index];
                if (p.From == CurrentState && p.Name == name)
                {
                    transition = p;
                    break;
                }
            }

            if (transition != null)
            {
                Transition(transition);
            }
        }

        public void Transition(StateTransition transition)
        {
            if (transition.From == CurrentState)
            {
                CurrentState = transition.To;
                LastTransition = transition;
            }
                
        }

        public StateTransition LastTransition { get; set; }
    }

    public class StateTransition
    {
        public string Identifier;
        public State From;
        public State To;
        public string Name;


        public StateTransition(string name, State from, State to)
        {
            Name = name;
            From = from;
            To = to;
        }
    }
}

