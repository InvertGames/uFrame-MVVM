using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns f rounded to the nearest integer.")]
[UBCategory("Mathf")]
public class RoundToInt : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.RoundToInt(_F.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Round {0} to int",  _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.RoundToInt(#_F#)");
	}

}