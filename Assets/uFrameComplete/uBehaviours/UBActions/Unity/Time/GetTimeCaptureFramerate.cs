using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"If captureFramerate is set to a value larger than 0, time will advance in")]
[UBCategory("Time")]
public class GetTimeCaptureFramerate : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.captureFramerate);
		}

	}

	public override string ToString(){
	return string.Format("Get captureFramerate from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.captureFramerate");
	}

}