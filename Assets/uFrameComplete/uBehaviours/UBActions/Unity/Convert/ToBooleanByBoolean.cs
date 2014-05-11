using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Convert")]
public class ToBooleanByBoolean : UBConditionAction {

	public UBBool _Value = new UBBool();
	public override bool PerformCondition(IUBContext context){
return Convert.ToBoolean(_Value.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
	return sb.Expression("Convert.ToBoolean(#_Value#)");
	}

}