using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Move keyboard focus to a named control.")]
[UBCategory("GUI")]
public class FocusControl : UBAction {

	
	[UBRequired] public UBString _Name = new UBString();
	protected override void PerformExecute(IUBContext context){
		GUI.FocusControl(_Name.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.FocusControl(#_Name#)");
	}

}