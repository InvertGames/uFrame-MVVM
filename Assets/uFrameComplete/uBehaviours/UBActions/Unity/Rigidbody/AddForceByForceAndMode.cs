using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a force to the rigidbody. As a result the rigidbody will start moving.")]
[UBCategory("Rigidbody")]
public class AddForceByForceAndMode : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Force = new UBVector3();
	
	[UBRequired] public UBEnum _Mode = new UBEnum(typeof(ForceMode));
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddForce(_Force.GetValue(context),((ForceMode)_Mode.GetIntValue(context)));
	}

	public override string ToString(){
	return string.Format("AddForce {0} and {1}", _Rigidbody.ToString(RootContainer), _Force.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddForce(#_Force#, #_Mode#)");
	}

}