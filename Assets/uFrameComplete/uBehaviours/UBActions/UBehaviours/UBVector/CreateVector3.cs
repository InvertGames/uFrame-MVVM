using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBVector")]
public class CreateVector3 : UBAction {

	[UBRequired] public UBFloat _X = new UBFloat();
	[UBRequired] public UBFloat _Y = new UBFloat();
	[UBRequired] public UBFloat _Z = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBVector.CreateVector3(_X.GetValue(context),_Y.GetValue(context),_Z.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s CreateVector3 w/ {1}, {2} and {3}", "UBVector", _X.ToString(RootContainer), _Y.ToString(RootContainer), _Z.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBVector.CreateVector3(#_X#, #_Y#, #_Z#)");
	}

}