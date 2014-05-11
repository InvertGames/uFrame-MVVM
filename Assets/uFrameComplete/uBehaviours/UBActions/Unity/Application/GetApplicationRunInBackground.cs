using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Should the player be running when the application is in the background?")]
[UBCategory("Application")]
public class GetApplicationRunInBackground : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.runInBackground		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.runInBackground");
	}

}