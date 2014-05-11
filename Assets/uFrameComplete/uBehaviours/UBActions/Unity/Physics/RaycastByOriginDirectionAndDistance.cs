using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Casts a ray against all colliders in the scene.")]
[UBCategory("Physics")]
public class RaycastByOriginDirectionAndDistance : UBConditionAction {

	
	[UBRequired] public UBVector3 _Origin = new UBVector3();
	
	[UBRequired] public UBVector3 _Direction = new UBVector3();
	
	[UBRequired] public UBFloat _Distance = new UBFloat();
	public override bool PerformCondition(IUBContext context){
return Physics.Raycast(_Origin.GetValue(context),_Direction.GetValue(context),_Distance.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics.Raycast(#_Origin#, #_Direction#, #_Distance#)");
	}

}