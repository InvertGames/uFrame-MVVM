using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Convert")]
public class ToStringByValueAndToBase : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	[UBRequired] public UBInt _ToBase = new UBInt();
	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToString(_Value.GetValue(context),_ToBase.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s ToString w/ {1} and {2}", "Convert", _Value.ToString(RootContainer), _ToBase.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Convert.ToString(#_Value#, #_ToBase#)");
	}

}