using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The port of the connection tester used in .")]
[UBCategory("Network")]
public class SetNetworkConnectionTesterPort : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.connectionTesterPort = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s connectionTesterPort to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.connectionTesterPort = #_Value# ");
	}

}