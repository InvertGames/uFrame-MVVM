[UBCategory("Mouse")]
public class UBMouseUpTrigger : UBTrigger
{
    public static void OnMouseUp(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnMouseUp()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}