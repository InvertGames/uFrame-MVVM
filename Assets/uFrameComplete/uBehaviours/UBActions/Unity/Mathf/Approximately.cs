using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Compares two floating point values if they are similar.")]
[UBCategory("Mathf")]
public class Approximately : UBConditionAction {

	
	[UBRequired] public UBFloat _A = new UBFloat();
	
	[UBRequired] public UBFloat _B = new UBFloat();
	public override bool PerformCondition(IUBContext context){
return Mathf.Approximately(_A.GetValue(context),_B.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Mathf.Approximately(#_A#, #_B#)");
	}

}