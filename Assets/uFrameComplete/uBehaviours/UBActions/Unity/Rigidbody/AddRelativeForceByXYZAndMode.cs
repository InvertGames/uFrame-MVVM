using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Adds a force to the rigidbody relative to its coordinate system.")]
[UBCategory("Rigidbody")]
public class AddRelativeForceByXYZAndMode : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBFloat _X = new UBFloat();
	
	[UBRequired] public UBFloat _Y = new UBFloat();
	
	[UBRequired] public UBFloat _Z = new UBFloat();
	
	[UBRequired] public UBEnum _Mode = new UBEnum(typeof(ForceMode));
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddRelativeForce(_X.GetValue(context),_Y.GetValue(context),_Z.GetValue(context),((ForceMode)_Mode.GetIntValue(context)));
	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddRelativeForce(#_X#, #_Y#, #_Z#, #_Mode#)");
	}

}