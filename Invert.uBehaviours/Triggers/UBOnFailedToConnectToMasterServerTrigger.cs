using System.Collections.Generic;
using UnityEngine;

[UBCategory("Networking")]
public class UBOnFailedToConnectToMasterServerTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnFailedToConnectToMasterServerTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "networkError", ValueType = typeof(NetworkConnectionError) };
    }

    public static void OnFailedToConnectToMasterServer(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnFailedToConnectToMasterServer(NetworkConnectionError error)
    {
        ExecuteSheetWithVars(new UBEnum((int)error, typeof(NetworkConnectionError)) { Name = "networkError" });
    }
}