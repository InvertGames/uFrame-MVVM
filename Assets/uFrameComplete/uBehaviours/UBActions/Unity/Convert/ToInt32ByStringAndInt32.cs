using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Convert")]
public class ToInt32ByStringAndInt32 : UBAction {

	public UBString _Value = new UBString();
	public UBInt _FromBase = new UBInt();
	[UBRequireVariable] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToInt32(_Value.GetValue(context),_FromBase.GetValue(context)));
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
	sb.AppendExpression("Convert.ToInt32(#_Value#, #_FromBase#)");
	}

}