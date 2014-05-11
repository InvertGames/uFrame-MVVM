using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default linear velocity, below which objects start going to sleep (default 0.15). Must be positive.")]
[UBCategory("Physics")]
public class SetPhysicsSleepVelocity : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.sleepVelocity = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s sleepVelocity to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.sleepVelocity = #_Value# ");
	}

}