using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics")]
public class CapsuleCastByPoint1Point2RadiusAndDirection : UBConditionAction {

	[UBRequired] public UBVector3 _Point1 = new UBVector3();
	[UBRequired] public UBVector3 _Point2 = new UBVector3();
	[UBRequired] public UBFloat _Radius = new UBFloat();
	[UBRequired] public UBVector3 _Direction = new UBVector3();
	public override bool PerformCondition(IUBContext context){
return Physics.CapsuleCast(_Point1.GetValue(context),_Point2.GetValue(context),_Radius.GetValue(context),_Direction.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Physics.CapsuleCast(#_Point1#, #_Point2#, #_Radius#, #_Direction#)");
	}

}