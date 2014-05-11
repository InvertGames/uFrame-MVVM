using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnTriggerExitTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnTriggerExitTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "triggerGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider", ValueType = typeof(Collider) };
    }
    public static void OnTriggerExit(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnTriggerExit(Collider c)
    {
        if (Sheet != null)
        {
            ExecuteSheetWithVars(
                     new UBGameObject(c.gameObject) { Guid = "triggerGameObject" },
                     new UBObject(c, typeof(Collider)) { Guid = "collider" }
                 );
        }
    }
}