using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The absolute path to the web player data file (Read Only).")]
[UBCategory("Application")]
public class GetApplicationAbsoluteURL : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.absoluteURL);
		}

	}

	public override string ToString(){
	return string.Format("Get absoluteURL from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.absoluteURL");
	}

}