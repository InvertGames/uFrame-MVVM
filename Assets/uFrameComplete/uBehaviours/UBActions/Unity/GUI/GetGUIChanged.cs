using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Have any controls changed the value of input data?")]
[UBCategory("GUI")]
public class GetGUIChanged : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return GUI.changed		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.changed");
	}

}