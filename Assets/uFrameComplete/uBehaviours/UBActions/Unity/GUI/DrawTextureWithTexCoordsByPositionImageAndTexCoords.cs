using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GUI")]
public class DrawTextureWithTexCoordsByPositionImageAndTexCoords : UBAction {

	[UBRequired] public UBRect _Position = new UBRect();
	[UBRequired] public UBTexture _Image = new UBTexture();
	[UBRequired] public UBRect _TexCoords = new UBRect();
	protected override void PerformExecute(IUBContext context){
		GUI.DrawTextureWithTexCoords(_Position.GetValue(context),_Image.GetValue(context),_TexCoords.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.DrawTextureWithTexCoords(#_Position#, #_Image#, #_TexCoords#)");
	}

}