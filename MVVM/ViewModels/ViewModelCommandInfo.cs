using System;

namespace uFrame.MVVM
{
    public class ViewModelCommandInfo
    {
        public ISignal Signal { get; private set; }
        [Obsolete]
        public ICommand Command { get; set; }

        public string Name { get; set; }

        public Type ParameterType { get; set; }

        [Obsolete]
        public ViewModelCommandInfo(string name, ICommand command)
        {
            Name = name;
            Command = command;
        }

        public ViewModelCommandInfo(string name, ISignal signal)
        {
            Signal = signal;
            Name = name;
        }
        [Obsolete]
        public ViewModelCommandInfo(Type parameterType, string name, ICommand command)
        {
            ParameterType = parameterType;
            Name = name;
            Command = command;
        }
    }
}
