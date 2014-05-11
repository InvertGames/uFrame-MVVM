using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The path to the web player data file relative to the html file (Read Only).")]
[UBCategory("Application")]
public class GetApplicationSrcValue : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.srcValue);
		}

	}

	public override string ToString(){
	return string.Format("Get srcValue from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.srcValue");
	}

}