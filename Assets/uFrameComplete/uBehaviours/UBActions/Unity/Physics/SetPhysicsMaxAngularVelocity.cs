using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default maximimum angular velocity permitted for any rigid bodies (default 7). Must be positive.")]
[UBCategory("Physics")]
public class SetPhysicsMaxAngularVelocity : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.maxAngularVelocity = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s maxAngularVelocity to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.maxAngularVelocity = #_Value# ");
	}

}