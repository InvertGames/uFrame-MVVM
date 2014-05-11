using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnCollisionEnterTrigger : UBTrigger
{

    public static IEnumerable<IUBVariableDeclare> UBOnCollisionEnterTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "collisionGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider", ValueType = typeof(Collider) };
    }

    public static void OnCollisionEnter(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (Sheet != null)
        {
            ExecuteSheetWithVars(
                    new UBGameObject(collision.gameObject) { Guid = "collisionGameObject" },
                    new UBObject( collision.collider, typeof(Collider),false) { Guid = "collider" }
                );
        }
    }
}