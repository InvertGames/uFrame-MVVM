using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a button that is active as long as the user holds it down.")]
[UBCategory("GUI")]
public class RepeatButtonByPositionAndText : UBConditionAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBString _Text = new UBString();
	public override bool PerformCondition(IUBContext context){
return GUI.RepeatButton(_Position.GetValue(context),_Text.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.RepeatButton(#_Position#, #_Text#)");
	}

}