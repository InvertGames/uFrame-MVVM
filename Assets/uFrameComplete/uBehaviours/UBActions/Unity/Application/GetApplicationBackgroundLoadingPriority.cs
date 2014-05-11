using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Priority of background loading thread.")]
[UBCategory("Application")]
public class GetApplicationBackgroundLoadingPriority : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(ThreadPriority));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.backgroundLoadingPriority);
		}

	}

	public override string ToString(){
	return string.Format("Get backgroundLoadingPriority from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.backgroundLoadingPriority");
	}

}