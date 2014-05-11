using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true when in any kind of player (Read Only).")]
[UBCategory("Application")]
public class GetApplicationIsPlaying : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.isPlaying		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.isPlaying");
	}

}