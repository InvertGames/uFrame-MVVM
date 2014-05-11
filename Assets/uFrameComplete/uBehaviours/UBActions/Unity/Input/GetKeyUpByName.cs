using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user releases the key identified by name.")]
[UBCategory("Input")]
public class GetKeyUpByName : UBConditionAction {

	
	[UBRequired] public UBString _Name = new UBString();
	public override bool PerformCondition(IUBContext context){
return Input.GetKeyUp(_Name.GetValue(context))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetKeyUp(#_Name#)");
	}

}