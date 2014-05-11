[UBCategory("Visibility")]
public class UBOnBecameVisibleTrigger : UBTrigger
{
    public static void OnBecameVisible(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnBecameVisible()
    {
        ExecuteSheet();
    }
}