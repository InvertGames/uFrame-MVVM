using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Input")]
public class SetInputImeCompositionMode : UBAction {

	[UBRequired] public UBEnum _Value = new UBEnum(typeof(IMECompositionMode));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Input.imeCompositionMode = ((IMECompositionMode)_Value.GetIntValue(context));
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s imeCompositionMode to {1}", "Input", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Input.imeCompositionMode = #_Value# ");
	}

}