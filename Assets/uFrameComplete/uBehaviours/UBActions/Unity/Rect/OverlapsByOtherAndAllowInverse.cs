using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rect")]
public class OverlapsByOtherAndAllowInverse : UBConditionAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequired] public UBRect _Other = new UBRect();
	[UBRequired] public UBBool _AllowInverse = new UBBool();
	public override bool PerformCondition(IUBContext context){
return _Rect.GetValue(context).Overlaps(_Other.GetValue(context),_AllowInverse.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rect#.Overlaps(#_Other#, #_AllowInverse#)");
	}

}