using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Mathf")]
public class Gamma : UBAction {

	
	[UBRequired] public UBFloat _Value = new UBFloat();
	
	[UBRequired] public UBFloat _Absmax = new UBFloat();
	
	[UBRequired] public UBFloat _Gamma = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Gamma(_Value.GetValue(context),_Absmax.GetValue(context),_Gamma.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Gamma(#_Value#, #_Absmax#, #_Gamma#)");
	}

}