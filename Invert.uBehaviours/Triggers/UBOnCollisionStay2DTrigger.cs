using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnCollisionStay2DTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnCollisionStay2DTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "collisionGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider2D", ValueType = typeof(Collider2D) };
    }
    public static void OnCollisionStay2D(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (Sheet != null)
        {
            ExecuteSheetWithVars(
                      new UBGameObject(collision.gameObject) { Guid = "collisionGameObject" },
                      new UBObject(collision.collider, typeof(Collider2D)) { Guid = "collider2D" }
                  );
        }
    }
}