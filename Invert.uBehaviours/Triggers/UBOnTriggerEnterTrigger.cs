using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnTriggerEnterTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnTriggerEnterTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "triggerGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider", ValueType = typeof(Collider) };
    }
    public static void OnTriggerEnter(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnTriggerEnter(Collider c)
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