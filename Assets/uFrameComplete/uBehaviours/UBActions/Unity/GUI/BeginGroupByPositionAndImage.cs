using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Begin a group. Must be matched with a call to .")]
[UBCategory("GUI")]
public class BeginGroupByPositionAndImage : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	protected override void PerformExecute(IUBContext context){
		GUI.BeginGroup(_Position.GetValue(context),_Image.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.BeginGroup(#_Position#, #_Image#)");
	}

}