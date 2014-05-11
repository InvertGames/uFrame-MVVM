using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Begin a group. Must be matched with a call to .")]
[UBCategory("GUI")]
public class BeginGroupByPosition : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	protected override void PerformExecute(IUBContext context){
		GUI.BeginGroup(_Position.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.BeginGroup(#_Position#)");
	}

}