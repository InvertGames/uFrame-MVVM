using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnTriggerExit2DTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnTriggerExit2DTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "triggerGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider2D", ValueType = typeof(Collider2D) };
    }
    public static void OnTriggerExit2D(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnTriggerExit2D(Collider2D c)
    {
        if (Sheet != null)
        {
            ExecuteSheetWithVars(
                     new UBGameObject(c.gameObject) { Guid = "triggerGameObject" },
                     new UBObject(c, typeof(Collider2D)) { Guid = "collider2D" }
                 );
        }
    }
}