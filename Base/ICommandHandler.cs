using System;
using UniRx;
[Obsolete]
public interface ICommandDispatcher : IObservable<CommandInfo>
{
    void ExecuteCommand(ICommand command, object argument, bool isChained = false);
}

[Obsolete]
public class CommandInfo
{
    public CommandInfo(ICommand command, object argument, bool isChained)
    {
        Command = command;
        Argument = argument;
        IsChained = isChained;
    }
 
    public ICommand Command { get; set; }
    public object Argument { get; set; }
    public bool IsChained { get; set; }
}