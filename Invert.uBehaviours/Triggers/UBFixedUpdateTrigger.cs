[UBCategory("Update")]
public class UBFixedUpdateTrigger : UBTrigger
{
    public static void FixedUpdate(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void FixedUpdate()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}