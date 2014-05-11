[UBCategory("Default")]
public class UBLateUpdateTrigger : UBTrigger
{
    public static void LateUpdate(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void LateUpdate()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}