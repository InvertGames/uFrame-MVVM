using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Time")]
public class SetTimeMaximumDeltaTime : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Time.maximumDeltaTime = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s maximumDeltaTime to {1}", "Time", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Time.maximumDeltaTime = #_Value# ");
	}

}