using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the cosine of angle f in radians.")]
[UBCategory("Mathf")]
public class Cos : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Cos(_F.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Cosine of angle {0} in radians", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Cos(#_F#)");
	}

}