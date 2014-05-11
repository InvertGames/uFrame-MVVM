using UBehaviours.Actions;

public interface IActionHandler
{
    void MoveDown(UBAction action);

    void MoveUp(UBAction action);

    void Remove(UBAction action);

    void ShowOptions(UBAction action);
}