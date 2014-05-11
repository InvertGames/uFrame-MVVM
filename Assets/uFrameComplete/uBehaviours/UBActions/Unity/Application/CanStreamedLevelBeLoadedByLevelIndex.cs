using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Can the streamed level be loaded?")]
[UBCategory("Application")]
public class CanStreamedLevelBeLoadedByLevelIndex : UBConditionAction {

	
	[UBRequired] public UBInt _LevelIndex = new UBInt();
	public override bool PerformCondition(IUBContext context){
return Application.CanStreamedLevelBeLoaded(_LevelIndex.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.CanStreamedLevelBeLoaded(#_LevelIndex#)");
	}

}