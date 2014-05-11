using System.Collections.Generic;

[UBCategory("Animation")]
public class UBOnAnimatorIKTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnAnimatorIKTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "layerIndex", ValueType = typeof(int) };
    }

    public static void OnAnimatorIK(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnAnimatorIK(int layerIndex)
    {
        ExecuteSheetWithVars(new UBInt(layerIndex) { Name = "layerIndex" });
    }
}