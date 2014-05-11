using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is any key or mouse button currently held down? (Read Only)")]
[UBCategory("Input")]
public class GetInputAnyKey : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Input.anyKey		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.anyKey");
	}

}