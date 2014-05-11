using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class GetInputMultiTouchEnabled : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.multiTouchEnabled		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.multiTouchEnabled");
	}

}