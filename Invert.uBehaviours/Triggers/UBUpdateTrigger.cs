[UBCategory("Update")]
public class UBUpdateTrigger : UBTrigger
{
    public static void Update(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void Update()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}