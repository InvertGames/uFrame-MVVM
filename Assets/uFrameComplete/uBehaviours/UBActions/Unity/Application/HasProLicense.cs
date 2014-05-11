using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Application")]
public class HasProLicense : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return Application.HasProLicense()		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Application.HasProLicense()");
	}

}