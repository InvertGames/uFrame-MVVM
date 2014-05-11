using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a window become the active window.")]
[UBCategory("GUI")]
public class FocusWindow : UBAction {

	
	[UBRequired] public UBInt _WindowID = new UBInt();
	protected override void PerformExecute(IUBContext context){
		GUI.FocusWindow(_WindowID.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.FocusWindow(#_WindowID#)");
	}

}