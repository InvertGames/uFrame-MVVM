using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the password for the server (for incoming connections).")]
[UBCategory("Network")]
public class GetNetworkIncomingPassword : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.incomingPassword);
		}

	}

	public override string ToString(){
	return string.Format("Get incomingPassword from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.incomingPassword");
	}

}