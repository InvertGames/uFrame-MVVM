using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default maximimum angular velocity permitted for any rigid bodies (default 7). Must be positive.")]
[UBCategory("Physics")]
public class GetPhysicsMaxAngularVelocity : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.maxAngularVelocity);
		}

	}

	public override string ToString(){
	return string.Format("Get maxAngularVelocity from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.maxAngularVelocity");
	}

}