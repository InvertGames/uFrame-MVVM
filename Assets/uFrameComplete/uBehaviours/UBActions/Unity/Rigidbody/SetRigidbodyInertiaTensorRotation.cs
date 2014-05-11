using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The rotation of the inertia tensor.")]
[UBCategory("Rigidbody")]
public class SetRigidbodyInertiaTensorRotation : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequired] public UBQuaternion _Value = new UBQuaternion();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Rigidbody.GetValueAs<Rigidbody>(context).inertiaTensorRotation = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s inertiaTensorRotation to {1}", _Rigidbody.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.inertiaTensorRotation = #_Value# ");
	}

}