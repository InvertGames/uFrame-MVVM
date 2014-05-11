using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a text or texture label on screen.")]
[UBCategory("GUI")]
public class LabelByPositionAndText : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBString _Text = new UBString();
	protected override void PerformExecute(IUBContext context){
		GUI.Label(_Position.GetValue(context),_Text.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.Label(#_Position#, #_Text#)");
	}

}