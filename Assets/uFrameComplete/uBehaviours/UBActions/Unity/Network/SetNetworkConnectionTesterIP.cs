using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The IP address of the connection tester used in .")]
[UBCategory("Network")]
public class SetNetworkConnectionTesterIP : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.connectionTesterIP = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s connectionTesterIP to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.connectionTesterIP = #_Value# ");
	}

}