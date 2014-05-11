using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Draw a texture within a rectangle.")]
[UBCategory("GUI")]
public class DrawTextureByPositionImageScaleModeAlphaBlendAndImageAspect : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	
	[UBRequired] public UBEnum _ScaleMode = new UBEnum(typeof(ScaleMode));
	
	[UBRequired] public UBBool _AlphaBlend = new UBBool();
	
	[UBRequired] public UBFloat _ImageAspect = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		GUI.DrawTexture(_Position.GetValue(context),_Image.GetValue(context),((ScaleMode)_ScaleMode.GetIntValue(context)),_AlphaBlend.GetValue(context),_ImageAspect.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.DrawTexture(#_Position#, #_Image#, #_ScaleMode#, #_AlphaBlend#, #_ImageAspect#)");
	}

}