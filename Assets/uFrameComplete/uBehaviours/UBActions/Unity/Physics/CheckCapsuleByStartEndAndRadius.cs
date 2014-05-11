using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true if there are any colliders touching the capsule defined by the axis going from start and end and having radius in world coordinates")]
[UBCategory("Physics")]
public class CheckCapsuleByStartEndAndRadius : UBConditionAction {

	
	[UBRequired] public UBVector3 _Start = new UBVector3();
	
	[UBRequired] public UBVector3 _End = new UBVector3();
	
	[UBRequired] public UBFloat _Radius = new UBFloat();
	public override bool PerformCondition(IUBContext context){
return Physics.CheckCapsule(_Start.GetValue(context),_End.GetValue(context),_Radius.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics.CheckCapsule(#_Start#, #_End#, #_Radius#)");
	}

}