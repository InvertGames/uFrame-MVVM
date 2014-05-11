using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Convert")]
public class ToStringByInt32AndInt32 : UBAction {

	public UBInt _Value = new UBInt();
	public UBInt _ToBase = new UBInt();
	[UBRequireVariable] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToString(_Value.GetValue(context),_ToBase.GetValue(context)));
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
	sb.AppendExpression("Convert.ToString(#_Value#, #_ToBase#)");
	}

}