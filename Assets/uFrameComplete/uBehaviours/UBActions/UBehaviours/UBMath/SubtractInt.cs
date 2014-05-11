using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBMath")]
public class SubtractInt : UBAction {

	[UBRequired] public UBInt _A = new UBInt();
	[UBRequired] public UBInt _B = new UBInt();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBMath.SubtractInt(_A.GetValue(context),_B.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s SubtractInt w/ {1} and {2}", "UBMath", _A.ToString(RootContainer), _B.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBMath.SubtractInt(#_A#, #_B#)");
	}

}