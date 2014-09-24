using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Invert.StateMachine
{
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

        public State LastState
        {
            get { return LastValue as State; }
        }
        protected override void OnPropertyChanged(string value)
        {
            if (LastValue != null)
                LastState.OnExit(CurrentState);

            base.OnPropertyChanged(value);
            
            if (Value != null)
                Value.OnEnter(LastValue as State);
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


    public class StateMachineTrigger : IObserver<Unit>, IObserver<bool>
    {
        public StateMachine StateMachine { get; set; }

        public string Name { get; set; }

        public StateMachineTrigger(StateMachine stateMachine, string name)
        {
            StateMachine = stateMachine;
            Name = name;
        }

        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            
        }

        public void OnNext(bool value)
        {
            if (value)
                StateMachine.CurrentState.Trigger(this);

            Debug.Log(Name + " was triggered.");
        }

        public void OnNext(Unit value)
        {
            StateMachine.CurrentState.Trigger(this);

            Debug.Log(Name + " was triggered.");
        }

        
    }

    public class TestStateMachine : StateMachine
    {
        public TestStateMachine(ViewModel owner, string propertyName) : base(owner, propertyName)
        {
        }

        public StateMachineTrigger Next { get; set; }
        public StateMachineTrigger Back { get; set; }

         

    }


}

