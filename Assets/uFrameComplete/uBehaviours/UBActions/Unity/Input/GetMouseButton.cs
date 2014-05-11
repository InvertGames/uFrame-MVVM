using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns whether the the given mouse button is held down.")]
[UBCategory("Input")]
public class GetMouseButton : UBConditionAction {

	
	[UBRequired] public UBInt _Button = new UBInt();
	public override bool PerformCondition(IUBContext context){
return Input.GetMouseButton(_Button.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetMouseButton(#_Button#)");
	}

}