using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBVector")]
public class GetVector2Y : UBAction {

	[UBRequired] public UBVector2 _Vector = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBVector.GetVector2Y(_Vector.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s GetVector2Y w/ {1}", "UBVector", _Vector.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBVector.GetVector2Y(#_Vector#)");
	}

}