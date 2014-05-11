using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the largest integer smaller to or equal to f.")]
[UBCategory("Mathf")]
public class FloorToInt : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.FloorToInt(_F.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Floor of {0} as Int", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.FloorToInt(#_F#)");
	}

}