using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Moves the rigidbody to position.")]
[UBCategory("Rigidbody")]
public class MovePosition : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).MovePosition(_Position.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.MovePosition(#_Position#)");
	}

}