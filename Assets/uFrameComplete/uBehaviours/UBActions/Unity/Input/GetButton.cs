using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true while the virtual button identified by buttonName is held down.")]
[UBCategory("Input")]
public class GetButton : UBConditionAction {

	
	[UBRequired] public UBString _ButtonName = new UBString();
	public override bool PerformCondition(IUBContext context){
return Input.GetButton(_ButtonName.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetButton(#_ButtonName#)");
	}

}