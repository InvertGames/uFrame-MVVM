using System.Collections.Generic;

[UBCategory("Animation")]
public class UBOnJointBreakTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnJointBreakTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "breakForce", ValueType = typeof(float) };
    }

    public static void OnJointBreak(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnJointBreak(float breakForce)
    {
        ExecuteSheetWithVars(new UBFloat(breakForce) { Guid = "breakForce" });
    }
}