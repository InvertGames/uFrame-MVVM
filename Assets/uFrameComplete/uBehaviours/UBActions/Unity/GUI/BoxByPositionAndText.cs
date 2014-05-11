using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a graphical box.")]
[UBCategory("GUI")]
public class BoxByPositionAndText : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBString _Text = new UBString();
	protected override void PerformExecute(IUBContext context){
		GUI.Box(_Position.GetValue(context),_Text.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.Box(#_Position#, #_Text#)");
	}

}