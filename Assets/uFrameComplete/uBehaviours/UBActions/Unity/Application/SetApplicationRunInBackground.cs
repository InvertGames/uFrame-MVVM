using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Should the player be running when the application is in the background?")]
[UBCategory("Application")]
public class SetApplicationRunInBackground : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Application.runInBackground = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s runInBackground to {1}", "Application", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.runInBackground = #_Value# ");
	}

}