using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a graphical box.")]
[UBCategory("GUI")]
public class BoxByPositionAndImage : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	protected override void PerformExecute(IUBContext context){
		GUI.Box(_Position.GetValue(context),_Image.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.Box(#_Position#, #_Image#)");
	}

}