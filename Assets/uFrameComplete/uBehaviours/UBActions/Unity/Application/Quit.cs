using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Quits the player application. Quit is ignored in the editor or the web player.")]
[UBCategory("Application")]
public class Quit : UBAction {

	protected override void PerformExecute(IUBContext context){
		Application.Quit();
	}

	public override string ToString(){
	return string.Format("Quit Application");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Application.Quit()");
	}

}