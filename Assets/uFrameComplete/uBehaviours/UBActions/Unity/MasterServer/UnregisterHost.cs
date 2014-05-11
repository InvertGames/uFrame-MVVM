using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Unregister this server from the master server.")]
[UBCategory("MasterServer")]
public class UnregisterHost : UBAction {

	protected override void PerformExecute(IUBContext context){
		MasterServer.UnregisterHost();
	}

	public override string ToString(){
        return string.Format("UnregisterHost");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.UnregisterHost()");
	}

}