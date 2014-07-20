using System.Collections.Generic;
using UnityEngine;

[UBCategory("Networking")]
public class UBOnFailedToConnectTrigger : UBTrigger
{
    public static IEnumerable<IUBVariableDeclare> UBOnFailedToConnectTriggerVariables()
    {
        yield return new UBStaticVariableDeclare() { Name = "networkError", ValueType = typeof(NetworkConnectionError), EnumType = typeof(NetworkConnectionError) };
    }

    public static void OnFailedToConnect(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public void OnFailedToConnect(NetworkConnectionError error)
    {
        ExecuteSheetWithVars(new UBEnum((int)error, typeof(NetworkConnectionError)) { Name = "networkError" });
    }
}