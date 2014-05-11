using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the password for the server (for incoming connections).")]
[UBCategory("Network")]
public class SetNetworkIncomingPassword : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.incomingPassword = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s incomingPassword to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.incomingPassword = #_Value# ");
	}

}