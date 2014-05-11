using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make an on/off toggle button.")]
[UBCategory("GUI")]
public class ToggleByPositionValueAndText : UBConditionAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBBool _Value = new UBBool();
	
	[UBRequired] public UBString _Text = new UBString();
	public override bool PerformCondition(IUBContext context){
return GUI.Toggle(_Position.GetValue(context),_Value.GetValue(context),_Text.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.Toggle(#_Position#, #_Value#, #_Text#)");
	}

}