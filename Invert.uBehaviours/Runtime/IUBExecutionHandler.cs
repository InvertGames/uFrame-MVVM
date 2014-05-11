public interface IUBExecutionHandler
{
    void ExecuteSheet(IUBContext context, UBActionSheet sheet);

    void TriggerBegin(UBTrigger ubTrigger);

    void TriggerEnd(UBTrigger ubTrigger);
}