using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class SetVector2 : UBAction {

	[UBRequired] public UBVector2 _Vector2 = new UBVector2();
	[UBRequired] public UBFloat _New_x = new UBFloat();
	[UBRequired] public UBFloat _New_y = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Vector2.GetValue(context).Set(_New_x.GetValue(context),_New_y.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s Set w/ {1} and {2}", _Vector2.ToString(RootContainer), _New_x.ToString(RootContainer), _New_y.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Vector2#.Set(#_New_x#, #_New_y#)");
	}

}