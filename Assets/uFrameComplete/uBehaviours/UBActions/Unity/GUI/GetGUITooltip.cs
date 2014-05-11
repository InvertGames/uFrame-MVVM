using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The tooltip of the control the mouse is currently over, or which has keyboard focus. (Read Only).")]
[UBCategory("GUI")]
public class GetGUITooltip : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.tooltip);
		}

	}

	public override string ToString(){
	return string.Format("Get tooltip from {0}", "GUI");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = GUI.tooltip");
	}

}