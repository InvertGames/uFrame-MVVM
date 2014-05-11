using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBVector")]
public class CreateVector2 : UBAction {

	[UBRequired] public UBFloat _X = new UBFloat();
	[UBRequired] public UBFloat _Y = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBVector.CreateVector2(_X.GetValue(context),_Y.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s CreateVector2 w/ {1} and {2}", "UBVector", _X.ToString(RootContainer), _Y.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBVector.CreateVector2(#_X#, #_Y#)");
	}

}