using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is the GUI enabled?")]
[UBCategory("GUI")]
public class GetGUIEnabled : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return GUI.enabled		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("GUI.enabled");
	}

}