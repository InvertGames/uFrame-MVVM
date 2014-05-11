using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Resets all input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame.")]
[UBCategory("Input")]
public class ResetInputAxes : UBAction {

	protected override void PerformExecute(IUBContext context){
		Input.ResetInputAxes();
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Input.ResetInputAxes()");
	}

}