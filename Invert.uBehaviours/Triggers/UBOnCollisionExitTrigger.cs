using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnCollisionExitTrigger : UBTrigger
{

    public static IEnumerable<IUBVariableDeclare> UBOnCollisionExitTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "collisionGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider", ValueType = typeof(Collider) };
    }
    public static void OnCollisionExit(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnCollisionExit(Collision collision)
    {
        if (Sheet != null)
        {
            ExecuteSheetWithVars(
                   new UBGameObject(collision.gameObject) { Guid = "collisionGameObject" },
                   new UBObject(collision.collider, typeof(Collider)) { Guid = "collider" }
               );
        }
    }
}