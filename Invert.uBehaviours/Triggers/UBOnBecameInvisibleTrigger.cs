[UBCategory("Visibility")]
public class UBOnBecameInvisibleTrigger : UBTrigger
{
    public static void OnBecameInvisible(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnBecameInvisible()
    {
        ExecuteSheet();
    }
}