using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The version of the Unity runtime used to play the content.")]
[UBCategory("Application")]
public class GetApplicationUnityVersion : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.unityVersion);
		}

	}

	public override string ToString(){
	return string.Format("Get unityVersion from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.unityVersion");
	}

}