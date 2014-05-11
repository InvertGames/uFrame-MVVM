using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"A smoothed out  (Read Only).")]
[UBCategory("Time")]
public class GetTimeSmoothDeltaTime : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.smoothDeltaTime);
		}

	}

	public override string ToString(){
	return string.Format("Get smoothDeltaTime from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.smoothDeltaTime");
	}

}