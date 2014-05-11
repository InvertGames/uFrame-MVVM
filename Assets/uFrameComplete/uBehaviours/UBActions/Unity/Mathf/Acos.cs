using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the arc-cosine of f - the angle in radians whose cosine is f.")]
[UBCategory("Mathf")]
public class Acos : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Acos(_F.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Arc Cosine of {0}", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Acos(#_F#)");
	}

}