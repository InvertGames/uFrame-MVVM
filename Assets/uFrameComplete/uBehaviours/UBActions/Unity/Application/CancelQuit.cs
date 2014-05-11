using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Cancels quitting the application. This is useful for showing a splash screen at the end of a game.")]
[UBCategory("Application")]
public class CancelQuit : UBAction {

	protected override void PerformExecute(IUBContext context){
		Application.CancelQuit();
	}

	public override string ToString(){
	return string.Format("Cancel Application Quit ", "Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.CancelQuit()");
	}

}