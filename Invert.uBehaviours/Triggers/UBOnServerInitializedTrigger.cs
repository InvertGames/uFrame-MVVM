[UBCategory("Networking")]
public class UBOnServerInitializedTrigger : UBTrigger
{
    public static void OnServerInitialized(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnServerInitialized()
    {
        ExecuteSheet();
    }
}