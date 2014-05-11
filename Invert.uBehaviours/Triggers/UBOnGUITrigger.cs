[UBCategory("Default")]
public class UBOnGUITrigger : UBTrigger
{
    public static void OnGUI(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnGUI()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}