using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Are we running inside the Unity editor? (Read Only)")]
[UBCategory("Application")]
public class GetApplicationIsEditor : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.isEditor		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.isEditor");
	}

}