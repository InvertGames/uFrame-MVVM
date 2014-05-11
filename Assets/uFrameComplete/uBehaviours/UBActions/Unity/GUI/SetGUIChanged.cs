using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Have any controls changed the value of input data?")]
[UBCategory("GUI")]
public class SetGUIChanged : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.changed = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s changed to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.changed = #_Value# ");
	}

}