[UBCategory("Animation")]
public class UBOnAnimatorMoveTrigger : UBTrigger
{
    public static void OnAnimatorMove(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnAnimatorMove()
    {
        ExecuteSheet();
    }
}