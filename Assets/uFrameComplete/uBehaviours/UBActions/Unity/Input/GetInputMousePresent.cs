using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class GetInputMousePresent : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.mousePresent		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.mousePresent");
	}

}