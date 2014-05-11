using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user pressed the given mouse button.")]
[UBCategory("Input")]
public class GetMouseButtonDown : UBConditionAction {

	
	[UBRequired] public UBInt _Button = new UBInt();
	public override bool PerformCondition(IUBContext context){
return Input.GetMouseButtonDown(_Button.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetMouseButtonDown(#_Button#)");
	}

}