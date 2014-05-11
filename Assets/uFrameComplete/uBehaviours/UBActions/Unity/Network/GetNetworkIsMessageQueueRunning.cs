using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Enable or disable the processing of network messages.")]
[UBCategory("Network")]
public class GetNetworkIsMessageQueueRunning : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Network.isMessageQueueRunning		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Network.isMessageQueueRunning");
	}

}