[UBCategory("Networking")]
public class UBOnConnectedToServerTrigger : UBTrigger
{
    public static void OnConnectedToServer(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnConnectedToServer()
    {
        ExecuteSheet();
    }
}