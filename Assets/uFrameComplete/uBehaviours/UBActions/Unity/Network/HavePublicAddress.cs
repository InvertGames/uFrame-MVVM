using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Check if this machine has a public IP address.")]
[UBCategory("Network")]
public class HavePublicAddress : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Network.HavePublicAddress()		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Network.HavePublicAddress()");
	}

}