using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class GetInputCompensateSensors : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.compensateSensors		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.compensateSensors");
	}

}