using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The language the user's operating system is running in.")]
[UBCategory("Application")]
public class GetApplicationSystemLanguage : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(SystemLanguage));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Application.systemLanguage);
		}

	}

	public override string ToString(){
	return string.Format("Get systemLanguage from {0}", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Application.systemLanguage");
	}

}