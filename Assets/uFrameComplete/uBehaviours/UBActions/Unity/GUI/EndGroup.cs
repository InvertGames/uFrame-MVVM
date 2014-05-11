using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"End a group.")]
[UBCategory("GUI")]
public class EndGroup : UBAction {

	protected override void PerformExecute(IUBContext context){
		GUI.EndGroup();
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.EndGroup()");
	}

}