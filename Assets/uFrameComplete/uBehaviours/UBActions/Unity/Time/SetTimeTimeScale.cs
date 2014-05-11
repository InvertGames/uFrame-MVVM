using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The scale at which the time is passing. This can be used for slow motion effects.")]
[UBCategory("Time")]
public class SetTimeTimeScale : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Time.timeScale = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s timeScale to {1}", "Time", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Time.timeScale = #_Value# ");
	}

}