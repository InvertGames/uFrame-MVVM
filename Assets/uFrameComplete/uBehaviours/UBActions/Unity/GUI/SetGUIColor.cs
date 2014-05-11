using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Global tinting color for the GUI.")]
[UBCategory("GUI")]
public class SetGUIColor : UBAction {

	[UBRequired] public UBColor _Value = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.color = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s color to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.color = #_Value# ");
	}

}