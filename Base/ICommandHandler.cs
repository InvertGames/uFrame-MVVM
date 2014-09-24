public interface ICommandHandler
{
    void ExecuteCommand(ICommand command, object argument);
    void ExecuteCommand(ICommand command);
    void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument);
}