using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class GetApplicationGenuine : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.genuine		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.genuine");
	}

}