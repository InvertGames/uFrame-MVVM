using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class GetApplicationWebSecurityEnabled : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.webSecurityEnabled		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.webSecurityEnabled");
	}

}