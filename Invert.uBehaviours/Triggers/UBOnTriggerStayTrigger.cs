using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnTriggerStayTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnTriggerStayTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "triggerGameObject", ValueType = typeof(GameObject) };
        yield return new UBStaticVariableDeclare() { Name = "collider", ValueType = typeof(Collider) };
    }
    public static void OnTriggerStay(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnTriggerStay(Collider c)
    {
        if (Sheet != null)
        {
            ExecuteSheetWithVars(
                     new UBGameObject(c.gameObject) { Guid = "triggerGameObject" },
                     new UBObject( c,typeof(Collider)) { Guid = "collider" }
                 );
        }
    }
}