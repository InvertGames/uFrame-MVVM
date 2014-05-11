using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true the first frame the user releases the virtual button identified by buttonName.")]
[UBCategory("Input")]
public class GetButtonUp : UBConditionAction {

	
	[UBRequired] public UBString _ButtonName = new UBString();
	public override bool PerformCondition(IUBContext context){
return Input.GetButtonUp(_ButtonName.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetButtonUp(#_ButtonName#)");
	}

}