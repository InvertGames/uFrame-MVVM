using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Enable or disable the processing of network messages.")]
[UBCategory("Network")]
public class SetNetworkIsMessageQueueRunning : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.isMessageQueueRunning = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s isMessageQueueRunning to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.isMessageQueueRunning = #_Value# ");
	}

}