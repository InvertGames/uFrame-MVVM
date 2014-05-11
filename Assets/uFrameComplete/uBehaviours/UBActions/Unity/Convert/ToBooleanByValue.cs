using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Convert")]
public class ToBooleanByValue : UBConditionAction {

	[UBRequired] public UBString _Value = new UBString();
	public override bool PerformCondition(IUBContext context){
return Convert.ToBoolean(_Value.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Convert.ToBoolean(#_Value#)");
	}

}