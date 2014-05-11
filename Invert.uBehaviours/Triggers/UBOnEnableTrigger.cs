[UBCategory("Default")]
public class UBOnEnableTrigger : UBTrigger
{
    public static void OnEnable(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnEnable()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}