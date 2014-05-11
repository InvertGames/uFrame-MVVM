using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Can the streamed level be loaded?")]
[UBCategory("Application")]
public class CanStreamedLevelBeLoadedByLevelName : UBConditionAction {

	
	[UBRequired] public UBString _LevelName = new UBString();
	public override bool PerformCondition(IUBContext context){
return Application.CanStreamedLevelBeLoaded(_LevelName.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.CanStreamedLevelBeLoaded(#_LevelName#)");
	}

}