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
            set { Value = value; }
        }
        
        public virtual void Compose(List<State> states)
        {
            
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

