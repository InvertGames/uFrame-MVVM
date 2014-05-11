[UBCategory("Default")]
public class UBOnDestroyTrigger : UBTrigger
{
    public static void OnDestroy(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnDestroy()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}