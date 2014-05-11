using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a single press button. The user clicks them and something happens immediately.")]
[UBCategory("GUI")]
public class ButtonByPositionAndText : UBConditionAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBString _Text = new UBString();
	public override bool PerformCondition(IUBContext context){
        return GUI.Button(_Position.GetValue(context),_Text.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.Button(#_Position#, #_Text#)");
	}

}