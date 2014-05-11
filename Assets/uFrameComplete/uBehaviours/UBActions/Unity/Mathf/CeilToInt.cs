using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the smallest integer greater to or equal to f.")]
[UBCategory("Mathf")]
public class CeilToInt : UBAction {

	
	[UBRequired] public UBFloat _F = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.CeilToInt(_F.GetValue(context)));
		}

	}

	public override string ToString(){
        return string.Format("Celing of {0} as int", _F.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.CeilToInt(#_F#)");
	}

}