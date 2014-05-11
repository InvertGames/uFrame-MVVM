using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default send rate of network updates for all Network Views.")]
[UBCategory("Network")]
public class SetNetworkSendRate : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.sendRate = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s sendRate to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.sendRate = #_Value# ");
	}

}