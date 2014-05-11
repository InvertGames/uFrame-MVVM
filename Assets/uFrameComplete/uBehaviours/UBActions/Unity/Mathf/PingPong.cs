using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"PingPongs the value t, so that it is never larger than length and never smaller than 0.")]
[UBCategory("Mathf")]
public class PingPong : UBAction {

	
	[UBRequired] public UBFloat _T = new UBFloat();
	
	[UBRequired] public UBFloat _Length = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.PingPong(_T.GetValue(context),_Length.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.PingPong(#_T#, #_Length#)");
	}

}