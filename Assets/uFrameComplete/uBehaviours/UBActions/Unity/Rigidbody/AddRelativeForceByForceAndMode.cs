using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a force to the rigidbody relative to its coordinate system.")]
[UBCategory("Rigidbody")]
public class AddRelativeForceByForceAndMode : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Force = new UBVector3();
	
	[UBRequired] public UBEnum _Mode = new UBEnum(typeof(ForceMode));
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddRelativeForce(_Force.GetValue(context),((ForceMode)_Mode.GetIntValue(context)));
	}

	public override string ToString(){
	return string.Format("AddRelativeForce {0} and {1}", _Rigidbody.ToString(RootContainer), _Force.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddRelativeForce(#_Force#, #_Mode#)");
	}

}