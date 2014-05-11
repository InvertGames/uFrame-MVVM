using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rect")]
public class ContainsByPointAndAllowInverse : UBConditionAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequired] public UBVector3 _Point = new UBVector3();
	[UBRequired] public UBBool _AllowInverse = new UBBool();
	public override bool PerformCondition(IUBContext context){
return _Rect.GetValue(context).Contains(_Point.GetValue(context),_AllowInverse.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rect#.Contains(#_Point#, #_AllowInverse#)");
	}

}