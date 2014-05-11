using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Transform")]
public class GetTransformHasChanged : UBConditionAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	public override bool PerformCondition(IUBContext context){
return _Transform.GetValue(context).hasChanged		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Transform#.hasChanged");
	}

}