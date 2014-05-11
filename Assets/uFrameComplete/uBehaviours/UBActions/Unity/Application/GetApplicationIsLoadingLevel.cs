using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is some level being loaded? (Read Only)")]
[UBCategory("Application")]
public class GetApplicationIsLoadingLevel : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.isLoadingLevel		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.isLoadingLevel");
	}

}