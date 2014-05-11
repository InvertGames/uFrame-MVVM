using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The IP address of the master server.")]
[UBCategory("MasterServer")]
public class SetMasterServerIpAddress : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			MasterServer.ipAddress = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s ipAddress to {1}", "MasterServer", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.ipAddress = #_Value# ");
	}

}