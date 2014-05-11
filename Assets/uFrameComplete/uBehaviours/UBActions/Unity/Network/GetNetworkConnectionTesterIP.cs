using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The IP address of the connection tester used in .")]
[UBCategory("Network")]
public class GetNetworkConnectionTesterIP : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.connectionTesterIP);
		}

	}

	public override string ToString(){
	return string.Format("Get connectionTesterIP from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.connectionTesterIP");
	}

}