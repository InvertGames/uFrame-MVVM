using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The tooltip of the control the mouse is currently over, or which has keyboard focus. (Read Only).")]
[UBCategory("GUI")]
public class SetGUITooltip : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.tooltip = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s tooltip to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.tooltip = #_Value# ");
	}

}