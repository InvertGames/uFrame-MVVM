using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Report this machine as a dedicated server.")]
[UBCategory("MasterServer")]
public class SetMasterServerDedicatedServer : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			MasterServer.dedicatedServer = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s dedicatedServer to {1}", "MasterServer", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.dedicatedServer = #_Value# ");
	}

}