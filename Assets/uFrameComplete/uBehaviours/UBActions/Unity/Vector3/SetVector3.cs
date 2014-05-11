using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector3")]
public class SetVector3 : UBAction {

	[UBRequired] public UBVector3 _Vector3 = new UBVector3();
	[UBRequired] public UBFloat _New_x = new UBFloat();
	[UBRequired] public UBFloat _New_y = new UBFloat();
	[UBRequired] public UBFloat _New_z = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Vector3.GetValue(context).Set(_New_x.GetValue(context),_New_y.GetValue(context),_New_z.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s Set w/ {1}, {2} and {3}", _Vector3.ToString(RootContainer), _New_x.ToString(RootContainer), _New_y.ToString(RootContainer), _New_z.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Vector3#.Set(#_New_x#, #_New_y#, #_New_z#)");
	}

}