using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Priority of background loading thread.")]
[UBCategory("Application")]
public class SetApplicationBackgroundLoadingPriority : UBAction {

	[UBRequired] public UBEnum _Value = new UBEnum(typeof(ThreadPriority));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Application.backgroundLoadingPriority = ((ThreadPriority)_Value.GetIntValue(context));
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s backgroundLoadingPriority to {1}", "Application", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.backgroundLoadingPriority = #_Value# ");
	}

}