using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a window draggable.")]
[UBCategory("GUI")]
public class DragWindow : UBAction {

	protected override void PerformExecute(IUBContext context){
		GUI.DragWindow();
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.DragWindow()");
	}

}