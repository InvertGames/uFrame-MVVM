using System.Collections.Generic;
using UnityEngine;

[UBCategory("Collision")]
public class UBOnControllerColliderHitTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnControllerColliderHitTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "controllerColliderHit", ValueType = typeof(Vector3) };
        yield return new UBStaticVariableDeclare() { Name = "controllerColliderHitMoveLength", ValueType = typeof(float) };
        yield return new UBStaticVariableDeclare() { Name = "controllerColliderHitMoveDirection", ValueType = typeof(Vector3) };
    }

    public static void OnControllerColliderHit(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        ExecuteSheetWithVars(new UBVector3(hit.point) { Name = "controllerColliderHitPoint" });
        ExecuteSheetWithVars(new UBVector3(hit.moveDirection) { Name = "controllerColliderHitMoveLength" });
        ExecuteSheetWithVars(new UBFloat(hit.moveLength) { Name = "controllerColliderHitMoveDirection" });
    }
}