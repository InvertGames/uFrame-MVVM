using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the arc-tangent of f - the angle in radians whose tangent is f.")]
[UBCategory("Mathf")]
public class Atan : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Atan(_F.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Arc tangent of {0}", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Atan(#_F#)");
	}

}