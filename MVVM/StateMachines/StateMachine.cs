using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using UnityEngine;

namespace Invert.StateMachine
{
    public class StateMachine : P<State>
    {
        private List<State> _states;
        private List<StateTransition> _transitions;

        public StateMachine()
        {
        }

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
            set
            {
                Value = value;
            }
        }
        
        public virtual void Compose(List<State> states)
        {
            Transitions.Clear();
        }


        public virtual List<StateTransition> Transitions
        {
            get { return _transitions ?? (_transitions = new List<StateTransition>()); }
            set { _transitions = value; }
        }

        public void Transition(string name)
        {
            StateTransition transition = null;
            foreach (var t in Transitions)
            {
                if (t.From == CurrentState && t.Name == name)
                {
                    transition = t;
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
            if (transition == null) return;
            if (transition.From == CurrentState)
            {
                LastTransition = transition;
                CurrentState = transition.To;
               
            }
                
        }

        public void SetState<TState>() where TState : State
        {
            var state = States.OfType<TState>().FirstOrDefault();
            CurrentState = state;
        }
        public void SetState(string stateName)
        {
            var state = States.FirstOrDefault(p => p.Name == stateName);
            if (state != null)
            {
                CurrentState = state;
            }
        }
        public StateTransition LastTransition { get; set; }

        public string Identifier { get; private set; }

    }


    public class StateMachineTrigger : IObserver<Unit>, IObserver<bool>
    {
        private List<Func<bool>> _computers;
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
        }

        public void OnNext(Unit value)
        {
            StateMachine.CurrentState.Trigger(this);

        }

        public List<Func<bool>> Computers
        {
            get { return _computers ?? (_computers = new List<Func<bool>>()); }
            set { _computers = value; }
        }

        public void AddComputer(P<bool> computed)
        {
            computed.Subscribe(this);
            Computers.Add(computed.Computer);
        }
    }

}

