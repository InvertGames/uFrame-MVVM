using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a single press button. The user clicks them and something happens immediately.")]
[UBCategory("GUI")]
public class ButtonByPositionAndImage : UBConditionAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	public override bool PerformCondition(IUBContext context){
return GUI.Button(_Position.GetValue(context),_Image.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.Button(#_Position#, #_Image#)");
	}

}