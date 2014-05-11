using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Remove focus from all windows.")]
[UBCategory("GUI")]
public class UnfocusWindow : UBAction {

	protected override void PerformExecute(IUBContext context){
		GUI.UnfocusWindow();
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.UnfocusWindow()");
	}

}