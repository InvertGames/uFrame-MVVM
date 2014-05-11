using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Evaluates script snippet in the containing web page (Web Player only).")]
[UBCategory("Application")]
public class ExternalEval : UBAction {

	
	[UBRequired] public UBString _Script = new UBString();
	protected override void PerformExecute(IUBContext context){
		Application.ExternalEval(_Script.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s ExternalEval w/ {1}", "Application", _Script.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.ExternalEval(#_Script#)");
	}

}