using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Mathf")]
public class LinearToGammaSpace : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.LinearToGammaSpace(_Value.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.LinearToGammaSpace(#_Value#)");
	}

}