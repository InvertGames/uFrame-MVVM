using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class SetInputCompensateSensors : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Input.compensateSensors = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s compensateSensors to {1}", "Input", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Input.compensateSensors = #_Value# ");
	}

}