using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Instructs game to try to render at a specified frame rate.")]
[UBCategory("Application")]
public class SetApplicationTargetFrameRate : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Application.targetFrameRate = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s targetFrameRate to {1}", "Application", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.targetFrameRate = #_Value# ");
	}

}