using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics")]
public class GetIgnoreLayerCollision : UBConditionAction {

	[UBRequired] public UBInt _Layer1 = new UBInt();
	[UBRequired] public UBInt _Layer2 = new UBInt();
	public override bool PerformCondition(IUBContext context){
return Physics.GetIgnoreLayerCollision(_Layer1.GetValue(context),_Layer2.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics.GetIgnoreLayerCollision(#_Layer1#, #_Layer2#)");
	}

}