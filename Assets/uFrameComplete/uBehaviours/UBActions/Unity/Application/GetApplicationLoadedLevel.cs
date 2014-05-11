using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The level index that was last loaded (Read Only).")]
[UBCategory("Application")]
public class GetApplicationLoadedLevel : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.loadedLevel);
		}

	}

	public override string ToString(){
	return string.Format("Get loadedLevel from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.loadedLevel");
	}

}