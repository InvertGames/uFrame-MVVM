public interface ICommandDispatcher
{
    void ExecuteCommand(ICommand command, object argument);
}