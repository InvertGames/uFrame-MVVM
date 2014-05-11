using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user releases the key identified by name.")]
[UBCategory("Input")]
public class GetKeyUpByKey : UBConditionAction {

	
	[UBRequired] public UBEnum _Key = new UBEnum(typeof(KeyCode));
	public override bool PerformCondition(IUBContext context){
return Input.GetKeyUp(((KeyCode)_Key.GetIntValue(context)))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetKeyUp(#_Key#)");
	}

}