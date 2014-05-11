using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calculates the  parameter between of two values.")]
[UBCategory("Mathf")]
public class InverseLerp : UBAction {

	
	[UBRequired] public UBFloat _From = new UBFloat();
	
	[UBRequired] public UBFloat _To = new UBFloat();
	
	[UBRequired] public UBFloat _Value = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.InverseLerp(_From.GetValue(context),_To.GetValue(context),_Value.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.InverseLerp(#_From#, #_To#, #_Value#)");
	}

}