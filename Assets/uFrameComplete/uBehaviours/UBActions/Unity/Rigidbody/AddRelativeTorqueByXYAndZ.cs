using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a torque to the rigidbody relative to the rigidbodie's own coordinate system.")]
[UBCategory("Rigidbody")]
public class AddRelativeTorqueByXYAndZ : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBFloat _X = new UBFloat();
	
	[UBRequired] public UBFloat _Y = new UBFloat();
	
	[UBRequired] public UBFloat _Z = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddRelativeTorque(_X.GetValue(context),_Y.GetValue(context),_Z.GetValue(context));
	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddRelativeTorque(#_X#, #_Y#, #_Z#)");
	}

}