using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a torque to the rigidbody relative to the rigidbodie's own coordinate system.")]
[UBCategory("Rigidbody")]
public class AddRelativeTorqueByTorqueAndMode : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Torque = new UBVector3();
	
	[UBRequired] public UBEnum _Mode = new UBEnum(typeof(ForceMode));
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddRelativeTorque(_Torque.GetValue(context),((ForceMode)_Mode.GetIntValue(context)));
	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddRelativeTorque(#_Torque#, #_Mode#)");
	}

}