using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Draw a texture within a rectangle.")]
[UBCategory("GUI")]
public class DrawTextureByPositionAndImage : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	protected override void PerformExecute(IUBContext context){
		GUI.DrawTexture(_Position.GetValue(context),_Image.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.DrawTexture(#_Position#, #_Image#)");
	}

}