using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true while the user holds down the key identified by name. Think auto fire.")]
[UBCategory("Input")]
public class GetKeyByName : UBConditionAction {

	
	[UBRequired] public UBString _Name = new UBString();
	public override bool PerformCondition(IUBContext context){
return Input.GetKey(_Name.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetKey(#_Name#)");
	}

}