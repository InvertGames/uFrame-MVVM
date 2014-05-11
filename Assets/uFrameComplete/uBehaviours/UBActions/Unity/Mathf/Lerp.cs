using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Interpolates a towards b by t. t is clamped between 0 and 1.")]
[UBCategory("Mathf")]
public class Lerp : UBAction {

	[UBRequired] public UBFloat _From = new UBFloat();
	[UBRequired] public UBFloat _To = new UBFloat();
	
	[UBRequired] public UBFloat _T = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Lerp(_From.GetValue(context),_To.GetValue(context),_T.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Lerp {0} to {1}",  _From.ToString(RootContainer), _To.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Lerp(#_From#, #_To#, #_T#)");
	}

}