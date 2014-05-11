using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Convert")]
public class ToBooleanByString : UBConditionAction {

	public UBString _Value = new UBString();
	public override bool PerformCondition(IUBContext context){
return Convert.ToBoolean(_Value.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
	return sb.Expression("Convert.ToBoolean(#_Value#)");
	}

}