using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Rounds f to the next (larger) integer value.")]
[UBCategory("Mathf")]
public class Ceil : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Ceil(_F.GetValue(context)));
		}

	}

	public override string ToString(){
        return string.Format("Ceiling of {0}", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Ceil(#_F#)");
	}

}