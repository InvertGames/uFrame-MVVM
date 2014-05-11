using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Draw a texture within a rectangle.")]
[UBCategory("GUI")]
public class DrawTextureByPositionImageAndScaleMode : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	
	[UBRequired] public UBEnum _ScaleMode = new UBEnum(typeof(ScaleMode));
	protected override void PerformExecute(IUBContext context){
		GUI.DrawTexture(_Position.GetValue(context),_Image.GetValue(context),((ScaleMode)_ScaleMode.GetIntValue(context)));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.DrawTexture(#_Position#, #_Image#, #_ScaleMode#)");
	}

}