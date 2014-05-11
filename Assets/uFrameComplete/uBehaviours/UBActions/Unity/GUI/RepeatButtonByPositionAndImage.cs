using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a button that is active as long as the user holds it down.")]
[UBCategory("GUI")]
public class RepeatButtonByPositionAndImage : UBConditionAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBTexture _Image = new UBTexture();
	public override bool PerformCondition(IUBContext context){
return GUI.RepeatButton(_Position.GetValue(context),_Image.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.RepeatButton(#_Position#, #_Image#)");
	}

}