using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's ) are performed.")]
[UBCategory("Time")]
public class GetTimeFixedDeltaTime : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.fixedDeltaTime);
		}

	}

	public override string ToString(){
	return string.Format("Get fixedDeltaTime from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.fixedDeltaTime");
	}

}