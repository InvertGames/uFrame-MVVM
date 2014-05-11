using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Convert")]
public class ToInt32ByValue : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToInt32(_Value.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s ToInt32 w/ {1}", "Convert", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Convert.ToInt32(#_Value#)");
	}

}