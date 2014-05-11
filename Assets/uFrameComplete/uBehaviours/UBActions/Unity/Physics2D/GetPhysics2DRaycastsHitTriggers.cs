using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class GetPhysics2DRaycastsHitTriggers : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Physics2D.raycastsHitTriggers		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics2D.raycastsHitTriggers");
	}

}