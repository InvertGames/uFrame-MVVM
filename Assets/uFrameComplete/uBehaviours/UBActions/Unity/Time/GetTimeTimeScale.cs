using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The scale at which the time is passing. This can be used for slow motion effects.")]
[UBCategory("Time")]
public class GetTimeTimeScale : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.timeScale);
		}

	}

	public override string ToString(){
	return string.Format("Get timeScale from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.timeScale");
	}

}