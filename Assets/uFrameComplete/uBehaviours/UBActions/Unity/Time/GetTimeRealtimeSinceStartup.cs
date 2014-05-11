using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The real time in seconds since the game started (Read Only).")]
[UBCategory("Time")]
public class GetTimeRealtimeSinceStartup : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.realtimeSinceStartup);
		}

	}

	public override string ToString(){
	return string.Format("Get realtimeSinceStartup from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.realtimeSinceStartup");
	}

}