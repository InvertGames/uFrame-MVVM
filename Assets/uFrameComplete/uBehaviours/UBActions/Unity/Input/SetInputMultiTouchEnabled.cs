using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class SetInputMultiTouchEnabled : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Input.multiTouchEnabled = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s multiTouchEnabled to {1}", "Input", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Input.multiTouchEnabled = #_Value# ");
	}

}