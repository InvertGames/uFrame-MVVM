using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is the GUI enabled?")]
[UBCategory("GUI")]
public class SetGUIEnabled : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.enabled = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s enabled to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.enabled = #_Value# ");
	}

}