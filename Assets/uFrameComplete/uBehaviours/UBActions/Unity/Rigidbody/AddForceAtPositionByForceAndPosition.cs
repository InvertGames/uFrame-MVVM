using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Applies force at position. As a result this will apply a torque and force on the object.")]
[UBCategory("Rigidbody")]
public class AddForceAtPositionByForceAndPosition : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Force = new UBVector3();
	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddForceAtPosition(_Force.GetValue(context),_Position.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddForceAtPosition(#_Force#, #_Position#)");
	}

}