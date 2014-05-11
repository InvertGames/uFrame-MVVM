using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the logarithm of a specified number in a specified base.")]
[UBCategory("Mathf")]
public class LogByFAndP : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	
	[UBRequired] public UBFloat _P = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Log(_F.GetValue(context),_P.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Log(#_F#, #_P#)");
	}

}