using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true if your peer type is server.")]
[UBCategory("Network")]
public class GetNetworkIsServer : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Network.isServer		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Network.isServer");
	}

}