using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Indicate if proxy support is needed, in which case traffic is relayed through the proxy server.")]
[UBCategory("Network")]
public class GetNetworkUseProxy : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Network.useProxy		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Network.useProxy");
	}

}