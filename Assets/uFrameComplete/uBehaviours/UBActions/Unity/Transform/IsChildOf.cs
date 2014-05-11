using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is this transform a child of parent?")]
[UBCategory("Transform")]
public class IsChildOf : UBConditionAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBTransform _Parent = new UBTransform();
	public override bool PerformCondition(IUBContext context){
return _Transform.GetValue(context).IsChildOf(_Parent.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Transform#.IsChildOf(#_Parent#)");
	}

}