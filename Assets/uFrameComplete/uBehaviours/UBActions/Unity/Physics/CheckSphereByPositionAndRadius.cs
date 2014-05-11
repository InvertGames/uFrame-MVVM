using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true if there are any colliders touching the sphere defined by position and radius in world coordinates")]
[UBCategory("Physics")]
public class CheckSphereByPositionAndRadius : UBConditionAction {

	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	
	[UBRequired] public UBFloat _Radius = new UBFloat();
	public override bool PerformCondition(IUBContext context){
return Physics.CheckSphere(_Position.GetValue(context),_Radius.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics.CheckSphere(#_Position#, #_Radius#)");
	}

}