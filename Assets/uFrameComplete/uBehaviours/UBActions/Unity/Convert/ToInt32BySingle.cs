using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Convert")]
public class ToInt32BySingle : UBAction {

	public UBFloat _Value = new UBFloat();
	[UBRequireVariable] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Convert.ToInt32(_Value.GetValue(context)));
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
	sb.AppendExpression("Convert.ToInt32(#_Value#)");
	}

}