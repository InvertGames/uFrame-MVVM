using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns f raised to power p.")]
[UBCategory("Mathf")]
public class Pow : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	
	[UBRequired] public UBFloat _P = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Pow(_F.GetValue(context),_P.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("{0} Raised to Power {1}", _F.ToString(RootContainer), _P.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Pow(#_F#, #_P#)");
	}

}