using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user starts pressing down the key identified by name.")]
[UBCategory("Input")]
public class GetKeyDownByName : UBConditionAction {

	
	[UBRequired] public UBString _Name = new UBString();
	public override bool PerformCondition(IUBContext context){
return Input.GetKeyDown(_Name.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetKeyDown(#_Name#)");
	}

}