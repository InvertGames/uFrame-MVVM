namespace Invert.StateMachine
{
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