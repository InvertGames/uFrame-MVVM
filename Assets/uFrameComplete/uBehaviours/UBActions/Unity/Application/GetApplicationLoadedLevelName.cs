using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The name of the level that was last loaded (Read Only).")]
[UBCategory("Application")]
public class GetApplicationLoadedLevelName : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.loadedLevelName);
		}

	}

	public override string ToString(){
	return string.Format("Get loadedLevelName from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.loadedLevelName");
	}

}