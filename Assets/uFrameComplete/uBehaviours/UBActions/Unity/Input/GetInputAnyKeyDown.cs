using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true the first frame the user hits any key or mouse button (Read Only).")]
[UBCategory("Input")]
public class GetInputAnyKeyDown : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.anyKeyDown		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.anyKeyDown");
	}

}