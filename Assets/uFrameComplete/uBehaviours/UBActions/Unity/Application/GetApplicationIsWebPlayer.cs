using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class GetApplicationIsWebPlayer : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.isWebPlayer		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.isWebPlayer");
	}

}