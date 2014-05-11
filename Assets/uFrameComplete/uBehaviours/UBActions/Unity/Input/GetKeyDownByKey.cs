using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns true during the frame the user starts pressing down the key identified by name.")]
[UBCategory("Input")]
public class GetKeyDownByKey : UBConditionAction {

	
	[UBRequired] public UBEnum _Key = new UBEnum(typeof(KeyCode));
	public override bool PerformCondition(IUBContext context){
return Input.GetKeyDown(((KeyCode)_Key.GetIntValue(context)))		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("Input.GetKeyDown(#_Key#)");
	}

}