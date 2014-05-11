using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns square root of f.")]
[UBCategory("Mathf")]
public class Sqrt : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Sqrt(_F.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Sqrt of {0}", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Sqrt(#_F#)");
	}

}