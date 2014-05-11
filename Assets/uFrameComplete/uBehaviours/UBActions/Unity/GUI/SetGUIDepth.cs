using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The sorting depth of the currently executing GUI behaviour.")]
[UBCategory("GUI")]
public class SetGUIDepth : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			GUI.depth = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s depth to {1}", "GUI", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.depth = #_Value# ");
	}

}