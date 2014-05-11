[UBCategory("Mouse")]
public class UBMouseDownTrigger : UBTrigger
{
    public static void OnMouseDown(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnMouseDown()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}