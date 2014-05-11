using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Sets the mass based on the attached colliders assuming a constant density.")]
[UBCategory("Rigidbody")]
public class SetDensity : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBFloat _Density = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).SetDensity(_Density.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Set {0}'s Density to {1}", _Rigidbody.ToString(RootContainer), _Density.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.SetDensity(#_Density#)");
	}

}