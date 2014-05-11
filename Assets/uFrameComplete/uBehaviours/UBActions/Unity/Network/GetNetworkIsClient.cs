using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true if your peer type is client.")]
[UBCategory("Network")]
public class GetNetworkIsClient : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Network.isClient		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Network.isClient");
	}

}