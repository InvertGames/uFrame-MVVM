using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default angular velocity, below which objects start sleeping (default 0.14). Must be positive.")]
[UBCategory("Physics")]
public class SetPhysicsSleepAngularVelocity : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.sleepAngularVelocity = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s sleepAngularVelocity to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.sleepAngularVelocity = #_Value# ");
	}

}