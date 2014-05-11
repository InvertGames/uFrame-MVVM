using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default angular velocity, below which objects start sleeping (default 0.14). Must be positive.")]
[UBCategory("Physics")]
public class GetPhysicsSleepAngularVelocity : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.sleepAngularVelocity);
		}

	}

	public override string ToString(){
	return string.Format("Get sleepAngularVelocity from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.sleepAngularVelocity");
	}

}