using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Instructs game to try to render at a specified frame rate.")]
[UBCategory("Application")]
public class GetApplicationTargetFrameRate : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.targetFrameRate);
		}

	}

	public override string ToString(){
	return string.Format("Get targetFrameRate from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.targetFrameRate");
	}

}