using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class GetInputSimulateMouseWithTouches : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.simulateMouseWithTouches		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.simulateMouseWithTouches");
	}

}