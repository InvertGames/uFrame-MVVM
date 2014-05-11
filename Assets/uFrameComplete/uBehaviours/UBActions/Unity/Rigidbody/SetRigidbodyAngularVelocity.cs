using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The angular velocity vector of the rigidbody.")]
[UBCategory("Rigidbody")]
public class SetRigidbodyAngularVelocity : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequired] public UBVector3 _Value = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Rigidbody.GetValueAs<Rigidbody>(context).angularVelocity = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s angularVelocity to {1}", _Rigidbody.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.angularVelocity = #_Value# ");
	}

}