using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GUI")]
public class EndScrollViewByHandleScrollWheel : UBAction {

	[UBRequired] public UBBool _HandleScrollWheel = new UBBool();
	protected override void PerformExecute(IUBContext context){
		GUI.EndScrollView(_HandleScrollWheel.GetValue(context));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.EndScrollView(#_HandleScrollWheel#)");
	}

}