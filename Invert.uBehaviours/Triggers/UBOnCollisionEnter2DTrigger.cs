using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnCollisionEnter2DTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnCollisionEnter2DTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "collisionGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider2D", ValueType = typeof(Collider2D) };
    }
    public static void OnCollisionEnter2D(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnCollisionEnter2D(Collision2D collision)
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