using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"If captureFramerate is set to a value larger than 0, time will advance in")]
[UBCategory("Time")]
public class SetTimeCaptureFramerate : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Time.captureFramerate = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s captureFramerate to {1}", "Time", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Time.captureFramerate = #_Value# ");
	}

}