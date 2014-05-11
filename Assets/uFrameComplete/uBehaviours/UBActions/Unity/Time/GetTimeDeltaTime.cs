using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The time in seconds it took to complete the last frame (Read Only).")]
[UBCategory("Time")]
public class GetTimeDeltaTime : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.deltaTime);
		}

	}

	public override string ToString(){
	return string.Format("Get deltaTime from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.deltaTime");
	}

}