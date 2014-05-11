using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default linear velocity, below which objects start going to sleep (default 0.15). Must be positive.")]
[UBCategory("Physics")]
public class GetPhysicsSleepVelocity : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.sleepVelocity);
		}

	}

	public override string ToString(){
	return string.Format("Get sleepVelocity from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.sleepVelocity");
	}

}