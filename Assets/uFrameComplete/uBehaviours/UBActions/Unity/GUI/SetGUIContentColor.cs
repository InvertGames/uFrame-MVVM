using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Tinting color for all text rendered by the GUI.")]
[UBCategory("GUI")]
public class SetGUIContentColor : UBAction {

	[UBRequired] public UBColor _Value = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.contentColor = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s contentColor to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.contentColor = #_Value# ");
	}

}