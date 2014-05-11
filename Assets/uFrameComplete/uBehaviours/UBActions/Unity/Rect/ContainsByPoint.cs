using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true if the x and y components of point is a point inside this rectangle.")]
[UBCategory("Rect")]
public class ContainsByPoint : UBConditionAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	
	[UBRequired] public UBVector3 _Point = new UBVector3();
	public override bool PerformCondition(IUBContext context){
return _Rect.GetValue(context).Contains(_Point.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rect#.Contains(#_Point#)");
	}

}