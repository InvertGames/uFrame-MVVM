using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The total number of frames that have passed (Read Only).")]
[UBCategory("Time")]
public class GetTimeFrameCount : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.frameCount);
		}

	}

	public override string ToString(){
	return string.Format("Get frameCount from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.frameCount");
	}

}