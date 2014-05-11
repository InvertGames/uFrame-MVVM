using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a text or texture label on screen.")]
[UBCategory("GUI")]
public class LabelByPositionAndImage : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	protected override void PerformExecute(IUBContext context){
		GUI.Label(_Position.GetValue(context),_Image.GetValue(context));
	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.Label(#_Position#, #_Image#)");
	}

}