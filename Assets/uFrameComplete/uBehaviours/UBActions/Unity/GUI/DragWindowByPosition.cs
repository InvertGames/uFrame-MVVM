using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a window draggable.")]
[UBCategory("GUI")]
public class DragWindowByPosition : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	protected override void PerformExecute(IUBContext context){
		GUI.DragWindow(_Position.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.DragWindow(#_Position#)");
	}

}