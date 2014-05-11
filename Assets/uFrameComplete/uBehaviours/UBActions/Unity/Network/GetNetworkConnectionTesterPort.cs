using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The port of the connection tester used in .")]
[UBCategory("Network")]
public class GetNetworkConnectionTesterPort : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.connectionTesterPort);
		}

	}

	public override string ToString(){
	return string.Format("Get connectionTesterPort from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.connectionTesterPort");
	}

}