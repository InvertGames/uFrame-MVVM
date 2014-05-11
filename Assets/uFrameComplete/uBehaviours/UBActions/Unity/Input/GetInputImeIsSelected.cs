using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class GetInputImeIsSelected : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.imeIsSelected		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.imeIsSelected");
	}

}