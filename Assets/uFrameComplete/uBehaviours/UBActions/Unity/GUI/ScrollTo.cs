using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Scrolls all enclosing scrollviews so they try to make position visible.")]
[UBCategory("GUI")]
public class ScrollTo : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	protected override void PerformExecute(IUBContext context){
		GUI.ScrollTo(_Position.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.ScrollTo(#_Position#)");
	}

}