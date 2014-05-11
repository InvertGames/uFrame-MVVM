using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Interpolates between min and max and eases in and out at the limits.")]
[UBCategory("Mathf")]
public class SmoothStep : UBAction {

	
	[UBRequired] public UBFloat _From = new UBFloat();
	
	[UBRequired] public UBFloat _To = new UBFloat();
	
	[UBRequired] public UBFloat _T = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.SmoothStep(_From.GetValue(context),_To.GetValue(context),_T.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s SmoothStep w/ {1}, {2} and {3}", "Mathf", _From.ToString(RootContainer), _To.ToString(RootContainer), _T.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.SmoothStep(#_From#, #_To#, #_T#)");
	}

}