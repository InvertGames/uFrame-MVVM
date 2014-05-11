using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class GetApplicationGenuineCheckAvailable : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.genuineCheckAvailable		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.genuineCheckAvailable");
	}

}