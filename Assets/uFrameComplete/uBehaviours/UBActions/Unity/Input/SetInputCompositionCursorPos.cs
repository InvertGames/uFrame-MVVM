using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class SetInputCompositionCursorPos : UBAction {

	[UBRequired] public UBVector2 _Value = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Input.compositionCursorPos = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s compositionCursorPos to {1}", "Input", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Input.compositionCursorPos = #_Value# ");
	}

}