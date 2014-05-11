using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Bring a specific window to front of the floating windows.")]
[UBCategory("GUI")]
public class BringWindowToFront : UBAction {

	
	[UBRequired] public UBInt _WindowID = new UBInt();
	protected override void PerformExecute(IUBContext context){
		GUI.BringWindowToFront(_WindowID.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.BringWindowToFront(#_WindowID#)");
	}

}