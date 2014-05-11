using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user releases the given mouse button.")]
[UBCategory("Input")]
public class GetMouseButtonUp : UBConditionAction {

	
	[UBRequired] public UBInt _Button = new UBInt();
	public override bool PerformCondition(IUBContext context){
return Input.GetMouseButtonUp(_Button.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetMouseButtonUp(#_Button#)");
	}

}