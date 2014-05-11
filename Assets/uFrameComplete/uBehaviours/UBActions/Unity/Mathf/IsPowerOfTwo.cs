using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Mathf")]
public class IsPowerOfTwo : UBConditionAction {

	[UBRequired] public UBInt _Value = new UBInt();
	public override bool PerformCondition(IUBContext context){
return Mathf.IsPowerOfTwo(_Value.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Mathf.IsPowerOfTwo(#_Value#)");
	}

}