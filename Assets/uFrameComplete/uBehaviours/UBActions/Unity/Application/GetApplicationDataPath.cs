using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Contains the path to the game data folder (Read Only).")]
[UBCategory("Application")]
public class GetApplicationDataPath : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.dataPath);
		}

	}

	public override string ToString(){
	return string.Format("Get dataPath from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.dataPath");
	}

}