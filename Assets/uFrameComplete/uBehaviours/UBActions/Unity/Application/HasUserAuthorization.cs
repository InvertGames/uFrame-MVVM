using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class HasUserAuthorization : UBConditionAction {

	[UBRequired] public UBEnum _Mode = new UBEnum(typeof(UserAuthorization));
	public override bool PerformCondition(IUBContext context){
return Application.HasUserAuthorization(((UserAuthorization)_Mode.GetIntValue(context)))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.HasUserAuthorization(#_Mode#)");
	}

}