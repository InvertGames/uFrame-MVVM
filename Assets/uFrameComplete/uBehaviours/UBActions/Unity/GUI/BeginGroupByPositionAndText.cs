using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Begin a group. Must be matched with a call to .")]
[UBCategory("GUI")]
public class BeginGroupByPositionAndText : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBString _Text = new UBString();
	protected override void PerformExecute(IUBContext context){
		GUI.BeginGroup(_Position.GetValue(context),_Text.GetValue(context));
	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.BeginGroup(#_Position#, #_Text#)");
	}

}