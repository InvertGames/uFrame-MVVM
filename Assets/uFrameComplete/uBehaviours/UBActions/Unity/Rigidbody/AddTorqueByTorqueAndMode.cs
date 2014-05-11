using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a torque to the rigidbody.")]
[UBCategory("Rigidbody")]
public class AddTorqueByTorqueAndMode : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Torque = new UBVector3();
	
	[UBRequired] public UBEnum _Mode = new UBEnum(typeof(ForceMode));
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddTorque(_Torque.GetValue(context),((ForceMode)_Mode.GetIntValue(context)));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddTorque(#_Torque#, #_Mode#)");
	}

}