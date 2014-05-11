using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The angular velocity, below which objects start going to sleep.  (Default 0.14) range { 0, infinity }")]
[UBCategory("Rigidbody")]
public class GetRigidbodySleepAngularVelocity : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).sleepAngularVelocity);
		}

	}

	public override string ToString(){
	return string.Format("Get sleepAngularVelocity from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.sleepAngularVelocity");
	}

}