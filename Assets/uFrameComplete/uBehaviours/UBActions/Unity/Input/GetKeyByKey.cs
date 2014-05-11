using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true while the user holds down the key identified by name. Think auto fire.")]
[UBCategory("Input")]
public class GetKeyByKey : UBConditionAction {

	
	[UBRequired] public UBEnum _Key = new UBEnum(typeof(KeyCode));
	public override bool PerformCondition(IUBContext context){
return Input.GetKey(((KeyCode)_Key.GetIntValue(context)))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetKey(#_Key#)");
	}

}