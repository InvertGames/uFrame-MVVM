using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Convert")]
public class ToStringByValue : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToString(_Value.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s ToString w/ {1}", "Convert", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Convert.ToString(#_Value#)");
	}

}