using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Convert")]
public class ToSingleBySingle : UBAction {

	public UBFloat _Value = new UBFloat();
	[UBRequireVariable] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToSingle(_Value.GetValue(context)));
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
	sb.AppendExpression("Convert.ToSingle(#_Value#)");
	}

}