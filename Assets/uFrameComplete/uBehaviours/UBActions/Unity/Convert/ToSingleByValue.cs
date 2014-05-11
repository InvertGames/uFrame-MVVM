using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Convert")]
public class ToSingleByValue : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToSingle(_Value.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s ToSingle w/ {1}", "Convert", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Convert.ToSingle(#_Value#)");
	}

}