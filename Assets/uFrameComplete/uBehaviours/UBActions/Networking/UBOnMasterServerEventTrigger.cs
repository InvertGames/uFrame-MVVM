using System.Collections.Generic;
using UnityEngine;

[UBCategory("Networking")]
public class UBOnMasterServerEventTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnMasterServerEventTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "masterServerEvent", ValueType = typeof(MasterServerEvent) };
    }

    public static void OnMasterServerEvent(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        
        ExecuteSheetWithVars(new UBEnum((int)masterServerEvent, typeof(MasterServerEvent)) { Name = "masterServerEvent" });
    }
}