using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's ) are performed.")]
[UBCategory("Time")]
public class SetTimeFixedDeltaTime : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Time.fixedDeltaTime = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s fixedDeltaTime to {1}", "Time", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Time.fixedDeltaTime = #_Value# ");
	}

}