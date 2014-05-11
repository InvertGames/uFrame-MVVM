using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Bring a specific window to back of the floating windows.")]
[UBCategory("GUI")]
public class BringWindowToBack : UBAction {

	
	[UBRequired] public UBInt _WindowID = new UBInt();
	protected override void PerformExecute(IUBContext context){
		GUI.BringWindowToBack(_WindowID.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.BringWindowToBack(#_WindowID#)");
	}

}